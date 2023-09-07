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

    Vector3 offset = new Vector3(0, 0.501999974f, -0.261000007f);
    Vector3 pistolPivotStartPosition;

    float swayTime = 0;
    [SerializeField] float swayPeriodSeconds = 1;
    [SerializeField] float swayAmplitude = 0.2f;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controls = player.GetComponent<GoodPlayerControls>();
        pistolPivotStartPosition = pistolPivot.localPosition;
    }

    protected override void UpdateWithPause() {
        float inputX = Input.GetAxisRaw("Mouse X");
        float inputY = Input.GetAxisRaw("Mouse Y");

        transform.position = controls.transform.position + offset;
        
        cameraVerticalRotation -=  inputY* rotationSpeed;
        cameraHorizontalRotation += inputX * rotationSpeed;

        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation,-maxAngle,maxAngle);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation+Vector3.up*cameraHorizontalRotation+Vector3.forward*cameraXRotation;

        controls.SetOrientation(new Vector3(0, cameraHorizontalRotation, cameraVerticalRotation));

        //Sway(inputX, inputY);

    }

    public void TiltCamera(float value) {
        StartCoroutine(ChangeCameraTilt(value));
    }

    IEnumerator ChangeCameraTilt(float value){
        int i = 0;
        while (i<15){
            i++;
            cameraXRotation += value / 15;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        
    }

    void Sway(float inputX,float inputY) {
        
        if (Input.GetKey(KeyCode.LeftControl)){
            // sway:
            swayTime += Time.deltaTime;

            float t = 2 * Mathf.PI * swayTime / swayPeriodSeconds;

            //pistolPivot.localPosition = pistolPivotStartPosition + new Vector3(Mathf.Sin(t) * swayAmplitude, -Mathf.Cos(2*t + 0.3f) * swayAmplitude/2, 0);
            pistolPivot.localPosition = pistolPivotStartPosition + new Vector3(Mathf.Sin(t) * swayAmplitude, -Mathf.Cos(2 * t + 0.2f) * swayAmplitude / 2, 0);
        }



    }
}
