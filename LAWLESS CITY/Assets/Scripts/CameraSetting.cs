using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    private float mouseSensitivity = 2.0f;
    private float cameraRotaiton;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.NPCInteraction)
            return;

        FPRotate();
    }

    private void FPRotate()
    {
        cameraRotaiton = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0f, cameraRotaiton, 0f);
    }

}