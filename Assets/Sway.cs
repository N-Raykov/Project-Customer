using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviourWithPause
{

    [SerializeField] float intensity;
    [SerializeField] float intensityZ;
    [SerializeField] float returnTime;

    Quaternion startRotation;

    private void Start(){
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    protected override void UpdateWithPause(){

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Quaternion rotationX = Quaternion.AngleAxis(-intensity * mouseX,Vector3.up);
        Quaternion rotationY = Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
        Quaternion rotationZ = Quaternion.AngleAxis(intensityZ*mouseX,Vector3.forward);
        Quaternion targetRotation = startRotation * rotationX * rotationY * rotationZ;

        transform.localRotation = Quaternion.Lerp(transform.localRotation,targetRotation,Time.deltaTime*returnTime);

    }
}