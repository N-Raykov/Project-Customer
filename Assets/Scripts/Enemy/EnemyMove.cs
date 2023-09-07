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

    private enum EnemyState
    {
        Aggro,
        PreferredRange,
        MinRange,
        Stunned
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private EnemyState currentState = EnemyState.Aggro;
    private float movementTimer;

    protected override void UpdateWithPause()
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
                //do nothing
                break;
        }
        Vector3 direction = player.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
