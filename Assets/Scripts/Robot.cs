using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviourWithPause
{
    NavMeshAgent agent;

    [SerializeField] float speed;

    [SerializeField] float stunAfterFall;
    [SerializeField] float heightOfFall;
    [SerializeField] float timeToCut;
    Rigidbody rb;

    float timeStartedCutting;

    [SerializeField] float startingVelocity;
    bool isActive = false;

    float startPosition;
    float currentPosition;
    float timeToWaitUntilStart;

    GameObject closestTree;
    Vector3 treeDirection;

    public float distanceToGround { get; set; }

    public enum RobotState
    {
        Walking,
        Cutting,
        Stunned,
        Paused
    }
    float stunDuration;

    public RobotState currentState { get; private set; }
    public Tree bigTree { get; private set; }

    private void Start(){
        currentState = RobotState.Walking;
        GetComponents();

        Fall();
    }

    void GetComponents()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        closestTree = FindClosestBigTree();
        bigTree = closestTree.GetComponent<Tree>();
        GameManager.robot = gameObject;
        transform.position = new Vector3(transform.position.x, heightOfFall, transform.position.z);
    }

    void Fall()
    {
        startPosition = transform.position.y;
        float velocity = startingVelocity + UnityEngine.Random.Range(5, 10) * ((UnityEngine.Random.Range(0, 2) == 0) ? -1 : 1);
        startingVelocity = 0;
        timeToWaitUntilStart = UnityEngine.Random.Range(1, 5);
        StartCoroutine(WaitToStart(timeToWaitUntilStart, velocity));
    }

    IEnumerator WaitToStart(float pTime, float pVelocity)
    {
        yield return new WaitForSeconds(pTime);
        startingVelocity = pVelocity;
        rb.AddForce(Vector3.down * startingVelocity, ForceMode.VelocityChange);
    }

    GameObject FindClosestBigTree()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("BigTree");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    protected override void UpdateWithPause()
    {
        ExtraStuff();

        if (agent.enabled == true)
        {
            HandleStates();
        }
    }

    void HandleStates()
    {
        switch (currentState)
        {
            case RobotState.Walking:

                Vector3 closestTreeTrunk = new Vector3(closestTree.transform.position.x, transform.position.y, closestTree.transform.position.z);

                agent.SetDestination(closestTreeTrunk);
                agent.speed = speed;

                treeDirection = closestTree.transform.position - transform.position;

                if (treeDirection.magnitude > Mathf.Epsilon && treeDirection.magnitude < Mathf.Infinity)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(treeDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
                    transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
                }

                if (Vector3.Distance(agent.destination, transform.position) < 1f)
                {
                    currentState = RobotState.Cutting;
                    agent.SetDestination(transform.position);
                    timeStartedCutting = Time.time;
                }

                break;

            case RobotState.Cutting:

                if (treeDirection.magnitude > Mathf.Epsilon && treeDirection.magnitude < Mathf.Infinity)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(treeDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
                    transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
                }

                if (Time.time - timeStartedCutting > timeToCut)
                {
                    bigTree.TakeDamage(-transform.forward);
                    agent.enabled = false;
                    GameManager.robot = null;
                }
                break;

            case RobotState.Stunned:
                if (Time.time >= stunDuration)
                {
                    currentState = RobotState.Walking;
                }
                else
                {
                    agent.destination = this.transform.position;

                    return;
                }
                break;

            case RobotState.Paused:

                agent.destination = this.transform.position;

                if (GameManager.gameIsPaused == false)
                {
                    currentState = RobotState.Walking;
                }
                return;
        }
    }

    void ExtraStuff()
    {
        if (GameManager.gameIsPaused == true)
        {
            currentState = RobotState.Paused;
        }

        if (isActive == false)
        {
            currentPosition = transform.position.y;
            float t = Mathf.Abs(currentPosition - startPosition) / heightOfFall;
            rb.velocity = new Vector3(0, -Mathf.Lerp(startingVelocity, 0f, t), 0);
        }

        if (bigTree == null)
        {
            Destroy(gameObject);
        }
    }

    public void GetStunned(float pDuration)
    {
        stunDuration = Time.time + pDuration;
        currentState = RobotState.Stunned;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BigTree")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Ground" && isActive == false)
        {
            agent.enabled = true;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            GetStunned(stunAfterFall);
            isActive = true;
        }
    }


}