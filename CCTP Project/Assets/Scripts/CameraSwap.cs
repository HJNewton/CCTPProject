using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    [Header("Camera Settings")]
    public GameObject mainCamera;
    public GameObject topCamera;
    public Light worldLight;

    bool topActive = false;

    void Update()
    {
        SwitchCamera();
    }

    void SwitchCamera()
    {
        if (topActive)
        {
            mainCamera.SetActive(false);
            topCamera.SetActive(true);
            worldLight.shadowStrength = 0;
        }

        if (!topActive)
        {
            mainCamera.SetActive(true);
            topCamera.SetActive(false);
            worldLight.shadowStrength = 1;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            topActive = !topActive;
        }
    }
}
