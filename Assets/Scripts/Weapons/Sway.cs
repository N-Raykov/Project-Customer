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

    Quaternion startRotation;

    private void Start(){
        startRotation = transform.localRotation;
        startPosition = transform.localPosition;
    }

    protected override void UpdateWithPause(){

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float multiplier;

        if (manager.CheckActiveGunisAiming())
            multiplier = aimingMultiplier;
        else
            multiplier = 1f;

        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        Quaternion rotationX = Quaternion.AngleAxis(-intensity * mouseX * multiplier,Vector3.up);
        Quaternion rotationY = Quaternion.AngleAxis(intensity * mouseY * multiplier, Vector3.right);
        Quaternion rotationZ = Quaternion.AngleAxis(-intensityZ * mouseX * multiplier, Vector3.forward) * Quaternion.AngleAxis(-intensityZ * moveDirection.x, Vector3.forward);
        Quaternion targetRotation = startRotation * rotationX * rotationY * rotationZ;


        Vector3 targetPosition = startPosition + new Vector3(-moveDirection.x, 0, -moveDirection.z)* moveIntensity * multiplier;
        transform.localRotation = Quaternion.Lerp(transform.localRotation,targetRotation,Time.deltaTime*returnTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition,targetPosition,Time.deltaTime*returnTime);

    }
}
