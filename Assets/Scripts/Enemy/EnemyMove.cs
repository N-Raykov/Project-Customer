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
    [SerializeField] float minRotationSpeed;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] float maxRotationTime;

    [System.NonSerialized] public NavMeshAgent agent;

    [Header("Attacks")]
    [SerializeField] EnemyGun gun;
    [SerializeField] float range;
    [SerializeField] float shotCD;
    float timeSinceLastShot;

    [Header("Spawn")]
    [SerializeField] float stunAfterFall;
    [SerializeField] float heightOfFall;
    [SerializeField] int treesRequired;
    bool isActive;
    bool hasSpawned;
    Rigidbody rb;

    private enum EnemyState
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
        player = GameObject.Find("Player");
        transform.position = new Vector3(transform.position.x, heightOfFall, transform.position.z);
    }

    private EnemyState currentState = EnemyState.Aggro;
    private float stunDuration;
    private float movementTimer;

    protected override void UpdateWithPause()
    {
        ExtraStuff();

        HandleStates();
    }

    void RotateAndShoot()
    {
        float distanceToPlayer = Vector3.Distance(gun.transform.position, player.transform.position);
        timeSinceLastShot -= Time.deltaTime;

        float minDistance = 3.0f;
        float maxDistance = 15.0f;
        float minValue = 2.2f;
        float maxValue = 2.4f;

        float t = Mathf.Clamp01((distanceToPlayer - minDistance) / (maxDistance - minDistance));
        float marginOfError = Mathf.Lerp(minValue, maxValue, t);

        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
        Vector3 predictedPlayerPosition = player.transform.position + playerVelocity * (distanceToPlayer / gun.projectileSpeed) * marginOfError;

        Vector3 direction = predictedPlayerPosition - gun.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

        float rotationSpeed = Mathf.Clamp(angleDifference / maxRotationTime, minRotationSpeed, maxRotationSpeed);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Debug.Log(direction);

        if (distanceToPlayer < range && timeSinceLastShot < 0.0f)
        {
            gun.Shoot();
            timeSinceLastShot = shotCD;
        }
    }

    void HandleStates()
    {
        switch (currentState)
        {
            case EnemyState.Aggro:
                if (Vector3.Distance(player.transform.position, transform.position) < aggroRange && Vector3.Distance(player.transform.position, transform.position) > preferredRange)
                {
                    agent.SetDestination(player.transform.position);
                    agent.speed = speed;
                }
                else
                {
                    currentState = EnemyState.PreferredRange;
                    movementTimer = Time.time;
                }
                break;

            case EnemyState.PreferredRange:
                if (Vector3.Distance(player.transform.position, transform.position) < preferredRange && Vector3.Distance(player.transform.position, transform.position) > minRange)
                {
                    agent.speed = strafeSpeed;
                    if (Time.time >= movementTimer)
                    {
                        RandomlySwitchDirection();
                        ResetMovementTimer();
                    }
                }
                else
                {
                    currentState = EnemyState.MinRange;
                }
                break;

            case EnemyState.MinRange:
                if (Vector3.Distance(player.transform.position, transform.position) < minRange)
                {
                    agent.speed = moonWalkSpeed;
                    Vector3 toPlayer = player.transform.position - transform.position;
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
                    if (agent.enabled == true)
                    {
                        agent.destination = this.transform.position;
                    }
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

        RotateAndShoot();
    }

    void ExtraStuff()
    {
        if (GameManager.fallenTrees == treesRequired && hasSpawned == false)
        {
            hasSpawned = true;
            rb.velocity *= 0;
        }
        else if (hasSpawned == false)
        {
            WaitForSpawn();
        }

        if (GameManager.gameIsPaused == true)
        {
            currentState = EnemyState.Paused;
        }
    }

    public void GetGravityGloved(float pDuration, float pHeight)
    {
        agent.baseOffset = Mathf.Lerp(0.5f, pHeight, pDuration);

        GetStunned(pDuration);
    }

    void WaitForSpawn()
    {
        agent.enabled = false;
        transform.position = new Vector3(transform.position.x, heightOfFall, transform.position.z);

        GetStunned(999);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && isActive == false)
        {
            agent.enabled = true;
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

    private void ResetMovementTimer()
    {
        movementTimer = Time.time + Random.Range(minStrafeDuration, maxStrafeDuration);
    }
}
