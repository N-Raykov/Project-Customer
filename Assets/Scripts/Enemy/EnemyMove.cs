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
    MeshRenderer meshRenderer;
    Rigidbody rb;
    Transform gunPivot;

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
        meshRenderer = GetComponent<MeshRenderer>();
        player = GameObject.Find("Player");
        gunPivot = gun.transform.parent.transform;
        transform.position = new Vector3(transform.position.x, heightOfFall, transform.position.z);
    }

    private EnemyState currentState = EnemyState.Aggro;
    [System.NonSerialized] public float stunDuration;
    private float strafeTimer;

    protected override void UpdateWithPause()
    {
        ExtraStuff();

        if(agent.enabled == true)
        {
            HandleStates();
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
                    strafeTimer = Time.time;
                }
                break;

            case EnemyState.PreferredRange:
                if (Vector3.Distance(player.transform.position, transform.position) < preferredRange && Vector3.Distance(player.transform.position, transform.position) > minRange)
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

        RotateAndShoot();
    }

    void RotateAndShoot()
    {
        float distanceToPlayer = Vector3.Distance(gun.transform.position, player.transform.position);
        timeSinceLastShot -= Time.deltaTime;

        float minDistance = 3.0f;
        float maxDistance = 15.0f;
        float minValue = 1.9f;
        float maxValue = 1.3f;

        float t = Mathf.Clamp01((distanceToPlayer - minDistance) / (maxDistance - minDistance));
        float marginOfError = Mathf.Lerp(minValue, maxValue, t);

        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
        Vector3 predictedPlayerPosition = player.transform.position + playerVelocity * (distanceToPlayer / gun.projectileSpeed) * marginOfError;

        Vector3 direction = predictedPlayerPosition - gun.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

        float rotationSpeed = Mathf.Clamp(angleDifference / maxRotationTime, minRotationSpeed, maxRotationSpeed);

        float currentToTargetRotation = Compare(targetRotation, transform.rotation);


        if(currentToTargetRotation < 50)
        {
            gunPivot.transform.rotation = Quaternion.Slerp(gunPivot.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 0.2f * Time.deltaTime);

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        

        if (distanceToPlayer < range && timeSinceLastShot < 0.0f)
        {
            gun.Shoot();
            timeSinceLastShot = shotCD;
        }
    }

    private float Compare(Quaternion quatA, Quaternion quatB)
    {
        return Quaternion.Angle(quatA, quatB);
    }


    void ExtraStuff()
    {
        if (GameManager.fallenTrees == treesRequired && hasSpawned == false)
        {
            hasSpawned = true;
            meshRenderer.enabled = true;
            rb.constraints &= ~RigidbodyConstraints.FreezePosition;
            rb.velocity *= 0;
        }
        else if (hasSpawned == false)
        {
            WaitForSpawn();
        }
        else if (rb.velocity.y > 40) 
        {
            rb.velocity *= 0.9f;
        }

        if (GameManager.gameIsPaused == true)
        {
            currentState = EnemyState.Paused;
        }
    }

    void WaitForSpawn()
    {
        agent.enabled = false;
        transform.position = new Vector3(transform.position.x, heightOfFall, transform.position.z);
        meshRenderer.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
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

    private void ResetStrafeTimer()
    {
        strafeTimer = Time.time + Random.Range(minStrafeDuration, maxStrafeDuration);
    }
}
