using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviourWithPause
{

    [SerializeField] GameObject player;
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
    [SerializeField] float rotationSpeed;
    NavMeshAgent agent;

    [Header("Attacks")]
    [SerializeField] EnemyGun gun;
    [SerializeField] float range;
    [SerializeField] float shotCD;
    float timeSinceLastShot;

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
        agent = GetComponent<NavMeshAgent>();
        ignorePausedState = true;
    }

    private EnemyState currentState = EnemyState.Aggro;
    private float stunDuration;
    private float movementTimer;

    protected override void UpdateWithPause()
    {
        if (GameManager.gameIsPaused == true)
        {
            currentState = EnemyState.Paused;
        }

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
                if(Time.time >= stunDuration)
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
        // Attacking
        float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        timeSinceLastShot -= Time.deltaTime;

        // Calculate the predicted position of the player based on their current velocity
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
        Vector3 predictedPlayerPosition = player.transform.position + playerVelocity * (distanceToPlayer / gun.projectileSpeed) * 2.3f;

        // Calculate the direction to the predicted player position
        Vector3 direction = predictedPlayerPosition - transform.position;

        // Rotate towards the predicted position
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (distanceToPlayer < range && timeSinceLastShot < 0.0f)
        {
            gun.Shoot();
            timeSinceLastShot = shotCD;
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
