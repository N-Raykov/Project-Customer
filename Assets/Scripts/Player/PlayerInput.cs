using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviourWithPause
{
    [SerializeField] Transform rotationPivot;
    Controls controls;
    public Vector3 moveDirection { get; private set; }
    public bool jumpInput { get; private set; }
    public bool shootInputHold { get; private set; }
    public bool shootInputClick { get; private set; }
    public bool reloadInput { get; private set; }
    public bool aimInput { get; private set; }
    public bool shopInput { get; private set; }
    public bool interactionInput { get; private set; }
    public bool skillInput { get; private set; }
    public bool spawnBot { get; private set; }
    public bool sprintInput { get; private set; }

    private void Awake(){
        ignorePausedState = true;
    }

    private void Start(){

        controls = Controls.controls;
        
    }

    protected override void UpdateWithPause(){
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDirection.Normalize();
        moveDirection = Quaternion.Euler(new Vector3(0, rotationPivot.transform.localEulerAngles.y, 0)) * moveDirection;
        jumpInput = Input.GetKey(controls.keyList["jump"]);
        shootInputHold = Input.GetKey(controls.keyList["shoot"]);
        shootInputClick = Input.GetKeyDown(controls.keyList["shoot"]);
        reloadInput = Input.GetKeyDown(controls.keyList["reload"]);
        aimInput = Input.GetKey(controls.keyList["aim"]);
        shopInput = Input.GetKeyUp(controls.keyList["shop"]);
        interactionInput = Input.GetKeyDown(controls.keyList["interact"]);
        skillInput = Input.GetKeyDown(controls.keyList["ability1"]);
        spawnBot = Input.GetKeyDown(controls.keyList["robotSpawn"]);
        sprintInput = Input.GetKey(controls.keyList["sprint"]);

    }

}
