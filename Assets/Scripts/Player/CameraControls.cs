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
    public float cameraVerticalRotation { get; set; }
    public float cameraHorizontalRotation { get; set; }
    float cameraXRotation = 0;


    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controls = player.GetComponent<GoodPlayerControls>();
    }

    protected override void UpdateWithPause() {
        float inputX = Input.GetAxisRaw("Mouse X");
        float inputY = Input.GetAxisRaw("Mouse Y");


        //cameraVerticalRotation -= inputY * rotationSpeed;
        //cameraHorizontalRotation += inputX * rotationSpeed;

        //cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -maxAngle, maxAngle);
        //transform.localEulerAngles = Vector3.right * cameraVerticalRotation + Vector3.up * cameraHorizontalRotation + Vector3.forward * cameraXRotation;


        cameraVerticalRotation -= inputY * rotationSpeed;
        cameraHorizontalRotation += inputX * rotationSpeed;

        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -maxAngle, maxAngle);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;
        player.localEulerAngles = Vector3.up * cameraHorizontalRotation + Vector3.forward * cameraXRotation;

    }

}
