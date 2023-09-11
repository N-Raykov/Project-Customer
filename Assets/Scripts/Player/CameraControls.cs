using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraControls : MonoBehaviourWithPause{
    GoodPlayerControls controls;

    [SerializeField] Transform pistolPivot;
    [SerializeField] Transform player;
    [SerializeField] float rotationSpeed;
    [SerializeField] float maxAngle;
    public float mouseY { get; private set; }
    public float mouseX { get; private set; }
    float cameraXRotation = 0;


    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controls = player.GetComponent<GoodPlayerControls>();
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 1 / 60f;

    }

    protected override void UpdateWithPause() {
        float inputX = Input.GetAxisRaw("Mouse X");
        float inputY = Input.GetAxisRaw("Mouse Y");


        //cameraVerticalRotation -= inputY * rotationSpeed;
        //cameraHorizontalRotation += inputX * rotationSpeed;

        //cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -maxAngle, maxAngle);
        //transform.localEulerAngles = Vector3.right * cameraVerticalRotation + Vector3.up * cameraHorizontalRotation + Vector3.forward * cameraXRotation;

        //mouseX += inputX * rotationSpeed;
        //mouseY -= inputY * rotationSpeed;

        mouseX = inputX * rotationSpeed;
        mouseY = inputY * rotationSpeed;

        cameraXRotation -= mouseY;
        cameraXRotation = Mathf.Clamp(cameraXRotation, -maxAngle, maxAngle);

        //transform.localRotation = Quaternion.Euler(cameraXRotation, 0, 0);
        player.Rotate(Vector3.up * mouseX);

    }

}
