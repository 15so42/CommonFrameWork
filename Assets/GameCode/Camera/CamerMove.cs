using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerMove : MonoBehaviour
{
    private bool holdRight = false;//按住鼠标左键
    public float clampXMin=-15;
    public float clampXMax=6;
    public float sensitivity = 10;
    public float androidSensitivity = -2;

    public static bool dragging = false;

    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        androidTargetPos = transform.position;
        mainCam=Camera.main;
    }

    // Update is called once per frame
    
    
    void Update()
    {
        
#if UNITY_ANDROID
        AndroidMove();
#elif UNITY_STANDALONE || UNITY_EDITOR
        PcMove();
#endif
        
        
    }

    void PcMove()
    {
        holdRight = Input.GetMouseButton(1);
        if (holdRight && !dragging)
        {
            var right = Input.GetAxis("Mouse X");
            
            var lastPos = transform.position;
            var newPos =lastPos + Vector3.right * (right * sensitivity * Time.deltaTime);
            
            
            newPos.x = Mathf.Clamp(newPos.x,clampXMin, clampXMax);
            transform.position = newPos;
           

        }
    }

    private Vector3 androidTargetPos=Vector3.zero;
    void AndroidMove()
    {
        // var targetPos=Vector3.Lerp(transform.position, androidTargetPos, 1 * Time.deltaTime);
        // targetPos.x=Mathf.Clamp(targetPos.x,clampXMin, clampXMax);
        // transform.position = targetPos;
        
        // 处理屏幕滑动事件
        if (Input.touchCount == 1 && !dragging)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                // 计算滑动距离
                float dragDistance = touch.deltaPosition.x / Screen.width;

                // 如果触摸位置在屏幕边缘的可触发拖动的区域内，则移动相机
                androidTargetPos=transform.position + new Vector3(dragDistance * mainCam.orthographicSize * mainCam.aspect, 0, 0) * androidSensitivity;
            }

           
        }
    }

    private void LateUpdate()
    {
       
        #if UNITY_ANDROID
        var targetPos=Vector3.Lerp(transform.position, androidTargetPos, 1 * Time.deltaTime);
        targetPos.x=Mathf.Clamp(targetPos.x,clampXMin, clampXMax);
        transform.position = targetPos;
        #endif
    }
}
