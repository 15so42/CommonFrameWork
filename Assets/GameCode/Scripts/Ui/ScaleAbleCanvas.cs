using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleAbleCanvas : MonoBehaviour,IScaleAbleUi
{
    private CanvasScaler canvasScaler;

    [Header("高宽比(0.5625=9/16)")]
    public float heightWidthRate = 0.5625f;
    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        EventCenter.AddListener<int>(EnumEventType.OnCanvasTargetWidthChange,ApplyWidth);
    }

    void Start()
    {
        ApplyWidth(GlobalVariables.globalCanvasWidth);
    }
    

    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EnumEventType.OnCanvasTargetWidthChange,ApplyWidth);
    }
    
    
    public void ApplyWidth(int width)
    {
        canvasScaler.referenceResolution = new Vector2(width, width * heightWidthRate);
    }
}
