using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviourWithPause
{
    [Header("Aim")]
    [SerializeField] float minRotationSpeed;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] float maxRotationTime;

    [Header("Attacks")]
    [SerializeField] EnemyGun gun;
    [SerializeField] float range;
    [SerializeField] float shotCD;
    float timeSinceLastShot;
    Transform gunPivot;

    [Header("PlayerAggro")]
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;
    [SerializeField] float aggroAtMinDistance;
    [SerializeField] float aggroAtMaxDistance;
    [SerializeField] float aggroOffset;
    [SerializeField] float aggroThreshold;

    GameObject player;

    EnemyMove enemy;

    Enemy enemyHealth;

    float playerAggro;

    [System.NonSerialized] public GameObject target;

    private void Start()
    {
        gunPivot = gun.transform.parent.transform;
        player = GameObject.Find("Player");
        enemy = GetComponent<EnemyMove>();
        enemyHealth = GetComponent<Enemy>();
    }

    protected override void UpdateWithPause()
    {
        RotateAndShoot();
    }

    void RotateAndShoot()
    {
        target = Targeting();

        if (enemy.currentState != enemy.stunnedState)
        {
            Aim(target);
        }
    }


    GameObject Targeting()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        float t = Mathf.Clamp01((distanceToPlayer - minDistance) / (maxDistance - minDistance));
        playerAggro = Mathf.Lerp(aggroAtMinDistance, aggroAtMaxDistance, t);

        if (GameManager.robot != null && playerAggro + enemyHealth.missingHealth + aggroOffset < aggroThreshold)
        {
            target = GameManager.robot;
        }
        else
        {
            target = player;
        }

        return target;
    }

    void Aim(GameObject target)
    {
        float distanceToTarget = Vector3.Distance(gun.transform.position, target.transform.position);
        timeSinceLastShot -= Time.deltaTime;

        float minDistance = 3.0f;
        float maxDistance = 15.0f;
        float minValue = 1.8f;
        float maxValue = 1.2f;

        float t = Mathf.Clamp01((distanceToTarget - minDistance) / (maxDistance - minDistance));
        float marginOfError = Mathf.Lerp(minValue, maxValue, t);

        Vector3 targetVelocity = target.GetComponent<Rigidbody>().velocity;
        Vector3 predictedTargetPosition = target.transform.position + targetVelocity * (distanceToTarget / gun.projectileSpeed) * marginOfError;

        Vector3 direction = predictedTargetPosition - gun.transform.position;

        if (direction.magnitude > Mathf.Epsilon && direction.magnitude < Mathf.Infinity)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

            float rotationSpeed = Mathf.Clamp(angleDifference / maxRotationTime, minRotationSpeed, maxRotationSpeed);

            float currentToTargetRotation = Compare(targetRotation, transform.rotation);

            if (currentToTargetRotation < 50)
            {
                gunPivot.transform.rotation = Quaternion.Slerp(gunPivot.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 0.2f * Time.deltaTime);

            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

            if (distanceToTarget < range && timeSinceLastShot < 0.0f)
            {
                gun.Shoot();
                timeSinceLastShot = shotCD;
            }
        }
    }

    private float Compare(Quaternion quatA, Quaternion quatB)
    {
        return Quaternion.Angle(quatA, quatB);
    }
}
