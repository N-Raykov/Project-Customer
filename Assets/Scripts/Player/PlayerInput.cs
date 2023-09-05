using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    GoodPlayerControls controls;
    Vector3 moveDirection;
    bool canJump;
    bool slowDownTime;
    bool canDash;
    bool canGrapple;
    bool canShootPistol1;
    bool canReload;
    bool canAim;
    public KeyCode grappleKey { get; private set; }
    public KeyCode shootKey { get; private set; }
    public KeyCode reloadKey { get; private set; }
    private void Awake(){
        if (GameSettings.grappleKey != KeyCode.None){
            grappleKey = GameSettings.grappleKey;
            shootKey = GameSettings.shootKey;
            reloadKey = GameSettings.reloadKey;
        }
        else {
            grappleKey = KeyCode.E;
            shootKey = KeyCode.Mouse0;
            reloadKey = KeyCode.R;
        }

    }
    private void Start(){
        controls = GetComponent<GoodPlayerControls>();
    }

    void Update(){
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDirection.Normalize();
        moveDirection = Quaternion.Euler(new Vector3(0, controls.GetOrientation().y, 0)) * moveDirection;
        canJump = Input.GetAxisRaw("Jump") == 1;
        slowDownTime = Input.GetButtonDown("Fire3");
        canDash = Input.GetButtonUp("Fire3");
        canGrapple = Input.GetKeyDown(grappleKey);
        canShootPistol1 = Input.GetKeyDown(shootKey);
        canReload = Input.GetKeyDown(reloadKey);
        canAim = Input.GetKey(KeyCode.Mouse1);
    }

    public Vector3 GetMoveDirection() {
        return moveDirection;
    }

    public bool CheckJumpInput() {
        return canJump;
    }
    public bool CheckTimeSlowInput() {
        return slowDownTime;
    }
    public bool CheckDashInput() {
        return canDash;
    }
    public bool CheckGrappleInput() {
        return canGrapple;
    }

    public bool CheckPistol1Input() {
        return canShootPistol1;
    }

    public bool CheckReloadInput() {
        return canReload;
    }

    public bool CheckAimInput() {
        return canAim;
    }
}
