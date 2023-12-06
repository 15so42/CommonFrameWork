using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerUi : MonoBehaviour
{
    public TMP_Text timerText;

    private float startTime;
    private bool timerActive = false;

    public static GameTimerUi instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startTime = Time.time;
        timerActive = true;
    }

    private void Update()
    {
        if (timerActive)
        {
            float timeElapsed = Time.time - startTime;
            string minutes = ((int)timeElapsed / 60).ToString("00");
            string seconds = (timeElapsed % 60).ToString("00");
            timerText.text = minutes + ":" + seconds;
        }
    }
}