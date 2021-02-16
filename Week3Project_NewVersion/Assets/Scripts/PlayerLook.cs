using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField]
    float sensitivity, xRotation, targetFov, baseFov, fovLerp;

    public Transform playerBody;
    public Camera mainCam;

    bool shiftdown;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //hide and lock cursor to screen center
        mainCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //mouse x func
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; //move mouse x
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; //move mouse y

        xRotation -= mouseY; //move camera x rotation to mouse y. using "+=" will invert axis.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //dont look behind the player.

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //apply the xRotation code to move.
        playerBody.Rotate(Vector3.up * mouseX); //only rotate sideways

        // mouse y func

        shiftdown = Input.GetKey(KeyCode.LeftShift);

        if (shiftdown)
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, targetFov, fovLerp);
        }
        else {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, baseFov, fovLerp);
        }
    }
}
