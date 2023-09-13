using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviourWithPause{

    [Header("Enable Bob")]
    [SerializeField] bool useHeadBob;
    [Header("Idle")]
    [SerializeField] float idleBobSpeed;
    [SerializeField] float idleBobAmount;
    [Header("Walk")]
    [SerializeField] float walkBobSpeed=0.5f;
    [SerializeField] float walkBobAmount=14f;

    [Header("Data")]
    [SerializeField] PlayerInput input;
    [SerializeField] float aimMultiplier;
    InteractionAndWeaponManager manager;

    float activeSpeed;
    float activeAmount;
    float startYPos;
    float timer;

    private void Start(){
        startYPos = transform.localPosition.y;
        manager = input.GetComponent<InteractionAndWeaponManager>();
    }

    protected override void UpdateWithPause() {
        if (useHeadBob)
            HandleHeadBob();

    
    }

    void HandleHeadBob() {
        if (input.moveDirection.magnitude != 0) {
            activeSpeed = walkBobSpeed;
            activeAmount = walkBobAmount;
        }
        else {
            activeSpeed = idleBobSpeed;
            activeAmount = idleBobAmount;
        }
        //Debug.Log(manager.CheckActiveGunisAiming());
        if (manager.CheckActiveGunisAiming()) {
            //activeSpeed *= aimMultiplier;
            activeAmount *= aimMultiplier;
        }

        timer += Time.deltaTime * activeSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x,startYPos+Mathf.Sin(timer)*activeAmount,transform.localPosition.z);
    }

}
