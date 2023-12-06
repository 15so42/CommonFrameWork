using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Serialization; // Import the DoTween namespace

public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler,IPointerExitHandler
{
    [FormerlySerializedAs("text")] public Graphic graphic;
    public float textScaler = 1.3f;
   
    private void Start()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlayAudio2D("HoverButton");

        // Use DoTween to scale the text with a 0.1s animation
        
        graphic.DOFade(1f, 0.1f); // Ensure the text is visible
        graphic.transform.DOScale(textScaler, 0.1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlayAudio2D("ButtonClick");
    }

    // Call this method to reset the text's scale back to its original size
    public void OnPointerExit(PointerEventData eventData)
    {
        graphic.DOFade(1f, 0.1f); // Ensure the text is visible
        graphic.transform.DOScale(1f, 0.1f);
    }

    private void OnDisable()
    {
        graphic.transform.localScale=Vector3.one;
    }
}