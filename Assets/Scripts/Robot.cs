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
    Tree bigTree;
    float timeStartedCutting;

    GameObject closestTree;
    Vector3 treeDirection;

    private enum RobotState 
    { 
        Walking,
        Cutting,
        Stunned,
        Paused
    }

    private RobotState currentState = RobotState.Walking;
    float stunDuration;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        closestTree = FindClosestBigTree();
        if (closestTree == null)
        {
            closestTree = this.gameObject;
        }
        else
        {
            bigTree = closestTree.GetComponent<Tree>();
        }
        GameManager.robot = gameObject;
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

                agent.SetDestination(closestTree.transform.position);
                agent.speed = speed;

                treeDirection = closestTree.transform.position - transform.position;

                if (treeDirection.magnitude > Mathf.Epsilon && treeDirection.magnitude < Mathf.Infinity)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(treeDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
                    transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
                }

                if (Vector3.Distance(closestTree.transform.position, transform.position) < 2.5f)
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
        /*
        if (rb.velocity.y > 40)
        {
            rb.velocity *= 0.9f;
        }
        */

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (GameManager.gameIsPaused == true)
        {
            currentState = RobotState.Paused;
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
            Debug.Log("L");
            Destroy(this.gameObject);
        }
    }
}
