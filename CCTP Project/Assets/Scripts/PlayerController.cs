﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    public bool lockCursor = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Movement(); 
        }

        // pressing esc toggles between hide/show
        if (Input.GetKeyDown(KeyCode.G))
        {
            lockCursor = !lockCursor;
        }

        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    void Movement()
    {
        rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * (Time.deltaTime / Time.timeScale);
        rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * (Time.deltaTime / Time.timeScale);
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * (Time.deltaTime / Time.timeScale);
            transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * (Time.deltaTime / Time.timeScale);
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * (Time.deltaTime / Time.timeScale);
            transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * (Time.deltaTime / Time.timeScale);
        }
        else
        {
            transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * (Time.deltaTime / Time.timeScale);
            transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * (Time.deltaTime / Time.timeScale);
        }

        if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * climbSpeed * (Time.deltaTime / Time.timeScale); }
        if (Input.GetKey(KeyCode.E)) { transform.position -= transform.up * climbSpeed * (Time.deltaTime / Time.timeScale); }
    }
}
