using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviourWithPause{

    [Header("Intensity")]
    [SerializeField] float intensity;
    [SerializeField] float intensityZ;
    [SerializeField] float returnTime;
    [SerializeField] float moveIntensity;

    [Header("Data")]
    [SerializeField] PlayerInput input;

    Vector3 startPosition;

    Quaternion startRotation;

    private void Start(){
        startRotation = transform.localRotation;
        startPosition = transform.localPosition;
    }

    protected override void UpdateWithPause(){

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //if (mouseX == 0 || mouseY == 0)
        //    return;

        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        Quaternion rotationX = Quaternion.AngleAxis(-intensity * mouseX,Vector3.up);
        Quaternion rotationY = Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
        Quaternion rotationZ = Quaternion.AngleAxis(-intensityZ * mouseX, Vector3.forward) * Quaternion.AngleAxis(-intensityZ * moveDirection.x, Vector3.forward);
        Quaternion targetRotation = startRotation * rotationX * rotationY * rotationZ;


        Vector3 targetPosition = startPosition + new Vector3(-moveDirection.x, 0, -moveDirection.z)*moveIntensity;
        transform.localRotation = Quaternion.Lerp(transform.localRotation,targetRotation,Time.deltaTime*returnTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition,targetPosition,Time.deltaTime*returnTime);

    }
}
