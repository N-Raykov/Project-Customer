using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoodPlayerControls : MonoBehaviourWithPause{
    Rigidbody rb;

    public enum State { 
        Walk,
        Jump,
        Sprint
    }
    public State state { get; private set; }
    State lastState;
    public int zone { get; set; }

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] float sprintSpeedMultiplier;

    public float speedMultiplier { get; set; }
    public float maxSpeedMultiplier { get; set; }
    public float normalMaxSpeedMultiplier { get; set; }
    public float normalSpeedMultiplier{ get; set; }

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float airSpeedMultiplier;


    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask ground;

    bool isGrounded = false;

    PlayerInput input;

    void Start(){
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        normalMaxSpeedMultiplier = 1;
        normalSpeedMultiplier = 1;
        maxSpeedMultiplier = normalMaxSpeedMultiplier;
        speedMultiplier = normalSpeedMultiplier;
        zone = -1;
    }
    protected override void FixedUpdateWithPause(){
        StateMachine();
    }

    protected override void UpdateWithPause(){
        isGrounded = Physics.SphereCast(new Ray( transform.position, Vector3.down ), 0.5f, playerHeight * 0.5f, ground);
        HandleState();
        LimitSpeed();
    }

    void MovePlayer() {
        rb.AddForce(input.moveDirection * moveSpeed*10*speedMultiplier,ForceMode.Force);
        LimitSpeed();
    }

    void LimitSpeed() {
        Vector3 flatVelocity = new Vector3(rb.velocity.x,0,rb.velocity.z);

        if (flatVelocity.magnitude > maxSpeed* maxSpeedMultiplier) {
            Vector3 limitedSpeed = flatVelocity.normalized * maxSpeed* maxSpeedMultiplier;
            rb.velocity = new Vector3(limitedSpeed.x,rb.velocity.y,limitedSpeed.z);
        }

    }

    void Jump() {
        if (input.jumpInput&&isGrounded){
            lastState = state;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void HandleState() {
        if (state == State.Walk)
            rb.drag = groundDrag;
        else 
            rb.drag = 0;
    }

    void StateMachine() {
        switch (state) {
            case State.Walk:
                MovePlayer();
                Jump();
                ChangeStateToJump();
                //ChangeStateToSprint();
                break;
            case State.Jump:
                MovePlayer();
                ChangeStateToWalk();
                break;
            case State.Sprint:
                MovePlayer();
                StopSprint();
                ChangeStateToJump();
                break;
        }
    
    }

    void ChangeStateToWalk() {
        if (isGrounded&&state!=State.Sprint){
            lastState = state;
            state = State.Walk;
            speedMultiplier = normalSpeedMultiplier;
            maxSpeedMultiplier = normalMaxSpeedMultiplier;
        }
    }

    void ChangeStateToJump() {
        if (!isGrounded){
            lastState = state;
            state = State.Jump;
            speedMultiplier = airSpeedMultiplier;
            maxSpeedMultiplier = airSpeedMultiplier;

        }
    }

    void ChangeStateToSprint() {
        if (isGrounded&&input.sprintInput) {
            Debug.Log(1);
            lastState = state;
            state = State.Sprint;
            speedMultiplier = sprintSpeedMultiplier;
            maxSpeedMultiplier = sprintSpeedMultiplier;
        }
    }

    void StopSprint() { 
        if (!input.sprintInput){
            lastState = state;
            state = State.Walk;
            speedMultiplier = normalSpeedMultiplier;
            maxSpeedMultiplier = normalMaxSpeedMultiplier;
        }
    
    }


}
