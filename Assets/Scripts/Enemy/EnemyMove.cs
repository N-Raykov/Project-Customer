using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviourWithPause
{

    GameObject player;
    [SerializeField] float minStrafeDuration;
    [SerializeField] float maxStrafeDuration;

    [Header("Ranges")]
    [SerializeField] float aggroRange;
    [SerializeField] float preferredRange;
    [SerializeField] float minRange;

    [Header("Speeds")]
    [SerializeField] float speed;
    [SerializeField] float strafeSpeed;
    [SerializeField] float moonWalkSpeed;
    
    [System.NonSerialized] public NavMeshAgent agent;

    [Header("Spawn")]
    [SerializeField] float stunAfterFall;
    [SerializeField] float heightOfFall;
    [SerializeField] float startingVelocity;
    [SerializeField] int treesRequired;

    float startPosition;
    float currentPosition;
    float timeToWaitUntilStart;

    [System.NonSerialized] public bool isActive;
    bool hasSpawned;
    MeshRenderer meshRenderer;
    Rigidbody rb;
    EnemyAim enemyAim;

    public enum EnemyState
    {
        Aggro,
        PreferredRange,
        MinRange,
        Stunned,
        Paused
    }

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        ignorePausedState = true;
        agent.enabled = false;
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        enemyAim = GetComponent<EnemyAim>();
        player = GameObject.Find("Player");
        transform.position = new Vector3(transform.position.x, heightOfFall, transform.position.z);
    }

    [System.NonSerialized] public EnemyState currentState = EnemyState.Aggro;
    [System.NonSerialized] public EnemyState stunnedState = EnemyState.Stunned;
    [System.NonSerialized] public float stunDuration;
    private float strafeTimer;

    protected override void UpdateWithPause()
    {
        ExtraStuff();

        if(agent.enabled == true)
        {
            HandleStates(enemyAim.target);
        }
    }

    void HandleStates(GameObject target)
    {
        switch (currentState)
        {
            case EnemyState.Aggro:
                if (Vector3.Distance(target.transform.position, transform.position) < aggroRange && Vector3.Distance(target.transform.position, transform.position) > preferredRange)
                {
                    agent.SetDestination(target.transform.position);
                    agent.speed = speed;
                }
                else
                {
                    currentState = EnemyState.PreferredRange;
                    strafeTimer = Time.time;
                }
                break;

            case EnemyState.PreferredRange:
                if (Vector3.Distance(target.transform.position, transform.position) < preferredRange && Vector3.Distance(target.transform.position, transform.position) > minRange)
                {
                    agent.speed = strafeSpeed;
                    if (Time.time >= strafeTimer)
                    {
                        RandomlySwitchDirection();
                        ResetStrafeTimer();
                    }
                }
                else
                {
                    currentState = EnemyState.MinRange;
                }
                break;

            case EnemyState.MinRange:
                if (Vector3.Distance(target.transform.position, transform.position) < minRange)
                {
                    agent.speed = moonWalkSpeed;
                    Vector3 toPlayer = target.transform.position - transform.position;
                    Vector3 targetPosition = transform.position - toPlayer.normalized * 5f;
                    agent.SetDestination(targetPosition);
                }
                else
                {
                    currentState = EnemyState.Aggro;
                }
                break;

            case EnemyState.Stunned:
                if (Time.time >= stunDuration)
                {
                    currentState = EnemyState.Aggro;
                }
                else
                {

                    agent.destination = this.transform.position;

                    return;
                }
                break;

            case EnemyState.Paused:

                agent.destination = this.transform.position;

                if (GameManager.gameIsPaused == false)
                {
                    currentState = EnemyState.Aggro;
                }
                return;
        }
    }

    void ExtraStuff()
    {
        if (GameManager.fallenTrees == treesRequired && hasSpawned == false)
        {
            hasSpawned = true;
            meshRenderer.enabled = true;

            startPosition = transform.position.y;
            float velocity = startingVelocity + UnityEngine.Random.Range(5, 10) * ((UnityEngine.Random.Range(0, 2) == 0) ? -1 : 1);
            startingVelocity = 0;
            timeToWaitUntilStart = UnityEngine.Random.Range(1, 5);
            StartCoroutine(WaitToStart(timeToWaitUntilStart, velocity));

        }
        else if (hasSpawned == false)
        {
            WaitForSpawn();
        }

        if(isActive == false)
        {
            currentPosition = transform.position.y;
            float t = Mathf.Abs(currentPosition - startPosition) / heightOfFall;
            rb.velocity = new Vector3(0, -Mathf.Lerp(startingVelocity, 0f, t), 0);
        }

        if (GameManager.gameIsPaused == true)
        {
            currentState = EnemyState.Paused;
        }
    }

    IEnumerator WaitToStart(float pTime, float pVelocity)
    {
        yield return new WaitForSeconds(pTime);
        startingVelocity = pVelocity;
        rb.AddForce(Vector3.down * startingVelocity, ForceMode.VelocityChange);
    }

    void WaitForSpawn()
    {
        agent.enabled = false;
        transform.position = new Vector3(transform.position.x, heightOfFall, transform.position.z);
        meshRenderer.enabled = false;
        GetStunned(999);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && isActive == false)
        {
            agent.enabled = true;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            GetStunned(stunAfterFall);
            isActive = true;
        }
    }
    
    public void GetStunned(float pDuration)
    {
        stunDuration = Time.time + pDuration;
        currentState = EnemyState.Stunned;
    }

    private void RandomlySwitchDirection()
    {
        float randomValue = Random.value;
        if (randomValue < 0.5f)
        {
            // Move left
            agent.destination = transform.position - transform.right * 5f;
        }
        else
        {
            // Move right
            agent.destination = transform.position + transform.right * 5f;
        }
    }

    private void ResetStrafeTimer()
    {
        strafeTimer = Time.time + Random.Range(minStrafeDuration, maxStrafeDuration);
    }
}
