using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviourWithPause{

    [Header("Intensity")]
    [SerializeField] float intensity;
    [SerializeField] float intensityZ;
    [SerializeField] float returnTime;
    [SerializeField] float moveIntensity;
    [SerializeField] float aimingMultiplier;

    [Header("Data")]
    [SerializeField] PlayerInput input;
    [SerializeField] InteractionAndWeaponManager manager;

    Vector3 startPosition;
    Vector3 targetPosition;

    Quaternion startRotation;
    Quaternion targetRotation;

    float mouseX;
    float mouseY;

    private void Start(){
        startRotation = transform.localRotation;
        startPosition = transform.localPosition;
        targetPosition = startPosition;
        targetRotation = startRotation;
    }


    protected override void UpdateWithPause(){

        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");


        float multiplier;

        if (manager.CheckActiveGunisAiming())
            multiplier = aimingMultiplier;
        else
            multiplier = 1f;

        float fpsMultiplier=(1/Time.deltaTime)/500;

        multiplier *= fpsMultiplier;

        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        Quaternion rotationX = Quaternion.AngleAxis(-intensity * mouseX * multiplier, Vector3.up);
        Quaternion rotationY = Quaternion.AngleAxis(intensity * mouseY * multiplier, Vector3.right);
        Quaternion rotationZ = Quaternion.AngleAxis(-intensityZ * mouseX * multiplier, Vector3.forward) * Quaternion.AngleAxis(-intensityZ * moveDirection.x, Vector3.forward);
        targetRotation = startRotation * rotationX * rotationY * rotationZ;
        targetPosition = startPosition + new Vector3(-moveDirection.x, 0, -moveDirection.z) * moveIntensity * multiplier / fpsMultiplier;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * returnTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * returnTime);
    }



}
