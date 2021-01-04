using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    public float timeOfDay;
    public int timeScale = 1;
    public TextMeshProUGUI timeText;

    private void Update()
    {
        ChangeTime();

        //timeText.text = timeOfDay.ToString("F0");
        //timeText.text = TimeSpan.FromSeconds(timeOfDay).ToString("mm:ss");

        var ts = TimeSpan.FromSeconds(timeOfDay);
        timeText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }

    void ChangeTime()
    {
        timeOfDay += Time.deltaTime * timeScale;
        timeOfDay %= 2400; // Clamp between 0-24
    }
}
