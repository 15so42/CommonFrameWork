using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public TMP_Text textUi;
    private void Awake()
    {
        EventCenter.AddListener<float>(EnumEventType.OnCoinChange,OnCoinChange);
    }
    

    void OnCoinChange(float newValue)
    {
        textUi.text = newValue.ToString("F1");
    }
}
