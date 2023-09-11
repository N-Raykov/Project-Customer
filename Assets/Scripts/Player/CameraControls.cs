using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraControls : MonoBehaviourWithPause{

    [SerializeField] Transform player;
    [SerializeField] float rotationSpeed;
    [SerializeField] float maxAngle;
    float cameraXRotation = 0;

    float cameraVerticalRotation;
    float cameraHorizontalRotation;


    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected override void UpdateWithPause() {
        float inputX = Input.GetAxisRaw("Mouse X");
        float inputY = Input.GetAxisRaw("Mouse Y");


        cameraVerticalRotation -= inputY * rotationSpeed;
        cameraHorizontalRotation += inputX * rotationSpeed;

        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -maxAngle, maxAngle);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation + Vector3.up * cameraHorizontalRotation + Vector3.forward * cameraXRotation;

    }

}
