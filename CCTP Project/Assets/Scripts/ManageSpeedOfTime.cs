using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSpeedOfTime : MonoBehaviour
{
    public int timeSpeed = 1;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            timeSpeed = 1;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
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
}
