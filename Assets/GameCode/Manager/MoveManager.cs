using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveManager : MonoBehaviour
{
    private Vector3 targetPos;
    public float moveSpeed = 1;

    public float stoppingDistance = 0.3f;

    private SpriteRenderer spriteRenderer;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private float moveAnimParam = 0;
    private float targetMoveAnimParam;//Move动画混合树过度
    private static readonly int SpeedAnimParam = Animator.StringToHash("Speed");


    /// <summary>
    /// 到达目的地Action，使用完后注意取消不用的方法
    /// </summary>
    public Action onArrive;

    private bool arrived = false;
    void Move()
    {
       
        var distance = Vector2.Distance(transform.position, targetPos);
        if (distance < stoppingDistance)
        {
            targetMoveAnimParam = 0;
            if (arrived == false)
            {
                onArrive?.Invoke();
                
                arrived = true;
            }
    
        }
        else
        {
            arrived = false;
            var delta = moveSpeed * Time.deltaTime;
            transform.position=Vector2.MoveTowards(transform.position, targetPos,delta);
            targetMoveAnimParam = 1;
        }
        moveAnimParam = Mathf.Lerp(moveAnimParam, targetMoveAnimParam, 10 * Time.deltaTime);
        animator.SetFloat(SpeedAnimParam,moveAnimParam);
    }

    public void SetTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    public void RotationControl()
    {
        if (!arrived)
        {
            var right = targetPos.x > transform.position.x;
            spriteRenderer.flipX = right;
        }
        
    }

   
    // Update is called once per frame
    void Update()
    {
        RotationControl();
        Move();
    }
}
