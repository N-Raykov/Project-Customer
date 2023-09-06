using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoodPlayerControls : MonoBehaviour{
    Rigidbody rb;

    public enum State { 
        Walk,
        Jump,
        WallRun,
        Dash,
        SlowDownTime,
        Grapple
    }
    public State state;
    public State lastState;


    [Header("Movement")]
    public float moveSpeed;
    public float maxSpeed;
    public float groundDrag;

    Vector3 orientation;
    [System.NonSerialized] public float speedMultiplier; // use get; set; instead
    [System.NonSerialized] public float maxSpeedMultiplier;
    [System.NonSerialized] public readonly float normalMaxSpeedMultiplier = 1;
    [System.NonSerialized] public readonly float normalSpeedMultiplier=1;


    [Header("Jump")]
    public float jumpForce;
    public float airSpeedMultiplier;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;

    bool isGrounded = false;


    [Header("Camera")]
    public float fov;
    public Vector3 lastPosition { get; private set; }

    PlayerInput input;
    public GameObject lastTriggerEntered { get; private set; }

    void Start(){
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        maxSpeedMultiplier = normalMaxSpeedMultiplier;
        speedMultiplier = normalSpeedMultiplier;
        lastPosition = transform.position;
    }
    void FixedUpdate()
    {
        lastPosition = transform.position;
        StateMachine();
    }

    private void Update(){
        isGrounded = Physics.SphereCast(new Ray( transform.position, Vector3.down ), 0.5f, playerHeight * 0.5f, ground);
        HandleState();
        LimitSpeed();
    }

    void MovePlayer() {
        rb.AddForce(input.GetMoveDirection() * moveSpeed*10*speedMultiplier,ForceMode.Force);
        LimitSpeed();
    }

    public void SetOrientation(Vector3 pOrientation) {
        orientation = pOrientation;
    }

    void LimitSpeed() {
        Vector3 flatVelocity = new Vector3(rb.velocity.x,0,rb.velocity.z);

        if (flatVelocity.magnitude > maxSpeed* maxSpeedMultiplier&&state!=State.Grapple) {
            Vector3 limitedSpeed = flatVelocity.normalized * maxSpeed* maxSpeedMultiplier;
            rb.velocity = new Vector3(limitedSpeed.x,rb.velocity.y,limitedSpeed.z);
        }

    }

    void Jump() {
        if (input.CheckJumpInput()&&isGrounded){
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
                break;
            case State.Jump:
                speedMultiplier = airSpeedMultiplier;
                MovePlayer();
                ChangeStateToWalk();
                break;
            case State.WallRun:

                break;
            case State.Dash:
                break;
            case State.SlowDownTime:
                MovePlayer();
                break;
            case State.Grapple:
                break;
        }
    
    }


    public Vector3 GetOrientation() {
        return orientation;
    }

    void ChangeStateToWalk() {
        if (isGrounded){
            lastState = state;
            state = State.Walk;
            speedMultiplier = normalSpeedMultiplier;
        }
    }

    void ChangeStateToJump() {
        if (!isGrounded){
            lastState = state;
            state = State.Jump;
            speedMultiplier = airSpeedMultiplier;
        }
    }

    public bool CheckIsGrounded() {
        return isGrounded;
    }

    private void OnTriggerEnter(Collider other){
        lastTriggerEntered = other.gameObject;
        if (other.gameObject.tag == "Death") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (other.gameObject.tag == "WinTrigger") {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("End Screen");
        }
    }

}
