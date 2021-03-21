using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManageSpeedOfTime : MonoBehaviour
{
    public int timeSpeed = 1;
    public TextMeshProUGUI scaleText;

    void Update()
    {
        scaleText.text = timeSpeed + "x";

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            timeSpeed = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            timeSpeed = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            timeSpeed = 5;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            timeSpeed = 10;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (timeSpeed != 0)
            {
                timeSpeed = 0;
            }
        }

        Time.timeScale = timeSpeed;
    }

    public void SpeedUpTime()
    {
        if (timeSpeed < 10)
        {
            if (timeSpeed == 1)
            {
                timeSpeed = 3;
            }

            if (timeSpeed == 3)
            {
                timeSpeed = 5;
            }

            if (timeSpeed == 5)
            {
                timeSpeed = 10;
            }
        }
    }

    public void SlowDownTime()
    {
        if (timeSpeed > 1)
        {
            if (timeSpeed == 1)
            {
                timeSpeed = 1;
            }

            if (timeSpeed == 3)
            {
                timeSpeed = 1;
            }

            if (timeSpeed == 5)
            {
                timeSpeed = 3;
            }

            if (timeSpeed == 10)
            {
                timeSpeed = 5;
            }
        }
    }
}
