using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviourWithPause
{
    GoodPlayerControls controls;
    public Vector3 moveDirection { get; private set; }
    public bool jumpInput { get; private set; }
    public bool shootInput { get; private set; }
    public bool reloadInput { get; private set; }
    public bool aimInput { get; private set; }
    public bool shopInput { get; private set; }
    public KeyCode shootKey { get; private set; }
    public KeyCode reloadKey { get; private set; }

    private void Awake(){
        ignorePausedState = true;
        if (GameSettings.grappleKey != KeyCode.None){
            shootKey = GameSettings.shootKey;
            reloadKey = GameSettings.reloadKey;
        }
        else {
            shootKey = KeyCode.Mouse0;
            reloadKey = KeyCode.R;
        }

    }
    private void Start(){
        controls = GetComponent<GoodPlayerControls>();
    }

    protected override void UpdateWithPause(){
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDirection.Normalize();
        moveDirection = Quaternion.Euler(new Vector3(0, controls.GetOrientation().y, 0)) * moveDirection;
        jumpInput = Input.GetAxisRaw("Jump") == 1;
        shootInput = Input.GetKeyDown(shootKey);
        reloadInput = Input.GetKeyDown(reloadKey);
        aimInput = Input.GetKey(KeyCode.Mouse1);
        shopInput = Input.GetKeyDown(KeyCode.H);
    }

}
