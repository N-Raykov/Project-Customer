using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviourWithPause
{
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

    public NavMeshAgent agent { get; private set; }

    [Header("Spawn")]
    [SerializeField] float stunAfterFall;
    [SerializeField] float heightOfFall;
    [SerializeField] float startingVelocity;

    [Header("Sound")]
    [SerializeField] AudioSource audioSourceThruster;

    [SerializeField] GameObject landingParticles;

    float startPosition;
    float currentPosition;
    float timeToWaitUntilStart;

    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform spawnPoint2;
    [SerializeField] GameObject thrusterPrefab;

    GameObject thruster;
    GameObject thruster2;

    public bool isActive { get; set; }
    Rigidbody rb;
    EnemyAim enemyAim;

    [SerializeField] Animator animator;
    const string isMoving = "IsMoving";

    public enum EnemyState
    {
        Aggro,
        PreferredRange,
        MinRange,
        Stunned,
        Aiming,
        Paused
    }

    [System.NonSerialized] public EnemyState currentState = EnemyState.Aggro;
    [System.NonSerialized] public EnemyState stunnedState = EnemyState.Stunned;
    [System.NonSerialized] public EnemyState aimingState = EnemyState.Aiming;

    public float stunDuration { get; set; }
    private float strafeTimer;

    void Start()
    {
        GetComponenets();

        GetStunned(999);

        Fall();
    }

    void GetComponenets()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        ignorePausedState = true;
        rb = GetComponent<Rigidbody>();
        enemyAim = GetComponent<EnemyAim>();
        transform.position = new Vector3(transform.position.x, transform.position.y + heightOfFall - 2f, transform.position.z);
        thruster = Instantiate(thrusterPrefab, spawnPoint.position, spawnPoint.rotation, transform);
        thruster2 = Instantiate(thrusterPrefab, spawnPoint2.position, spawnPoint2.rotation, transform);

        audioSourceThruster.Play();
    }

    void Fall()
    {
        startPosition = transform.position.y;
        float velocity = startingVelocity + UnityEngine.Random.Range(5, 10) * ((UnityEngine.Random.Range(0, 2) == 0) ? -1 : 1);
        startingVelocity = 0;
        timeToWaitUntilStart = UnityEngine.Random.Range(1, 5);
        StartCoroutine(WaitToStart(timeToWaitUntilStart, velocity));
    }

    protected override void UpdateWithPause()
    {
        ExtraStuff();

        if (agent.enabled == true && enemyAim.target != null)
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
                    animator.SetBool("isStrafing", true);
                    if (Time.time >= strafeTimer)
                    {
                        RandomlySwitchDirection();
                        ResetStrafeTimer();
                    }
                }
                else
                {
                    animator.SetBool("isStrafing", false);
                    ResetStrafeTimer();
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
                    animator.SetBool("isStunned", false);
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
        animator.SetBool(isMoving, agent.velocity.magnitude > Mathf.Epsilon);

        if (isActive == false)
        {
            currentPosition = transform.position.y;
            float t = Mathf.Abs(currentPosition - startPosition) / heightOfFall;
            rb.velocity = new Vector3(0, -Mathf.Lerp(startingVelocity, 0f, t), 0);
            if(thruster != null)
            {
                thruster.transform.localScale = new Vector3(Mathf.Lerp(2, 1f, t), Mathf.Lerp(2, 0.8f, t), Mathf.Lerp(2, 1f, t));
                thruster2.transform.localScale = new Vector3(Mathf.Lerp(2, 1f, t), Mathf.Lerp(2, 0.8f, t), Mathf.Lerp(2, 1f, t));
            }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && isActive == false)
        {
            agent.enabled = true;
            rb.useGravity = true;
            GetStunned(stunAfterFall);
            isActive = true;
            animator.SetTrigger("Land");
            Destroy(thruster);
            Destroy(thruster2);
            audioSourceThruster.Stop();
            Instantiate(landingParticles, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
        }
    }

    public void GetStunned(float pDuration)
    {
        stunDuration = Time.time + pDuration;
        currentState = EnemyState.Stunned;
        animator.SetBool("isStunned", true);
    }

    private void RandomlySwitchDirection()
    {
        float randomValue = Random.value;
        if (randomValue < 0.5f)
        {
            agent.destination = transform.position - transform.right * 5f;
            animator.SetBool("isStrafingLeft", false);
        }
        else
        {
            agent.destination = transform.position + transform.right * 5f;
            animator.SetBool("isStrafingLeft", true);
        }
    }

    private void ResetStrafeTimer()
    {
        strafeTimer = Time.time + Random.Range(minStrafeDuration, maxStrafeDuration);
    }
}
