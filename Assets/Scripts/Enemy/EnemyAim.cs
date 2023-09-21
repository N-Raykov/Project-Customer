using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviourWithPause
{
    [Header("Aim")]
    [SerializeField] float minRotationSpeed;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] float maxRotationTime;
    [SerializeField] float minDistanceAim;
    [SerializeField] float maxDistanceAim;
    [SerializeField] float minValue;
    [SerializeField] float maxValue;

    [Header("Attacks")]
    [SerializeField] float range;
    [SerializeField] EnemyLaser laser;
    [SerializeField] float laserCD;
    [SerializeField] EnemyBazooka bazooka;
    [SerializeField] float bazookaShotCD;

    [Header("PlayerAggro")]
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;
    [SerializeField] float aggroAtMinDistance;
    [SerializeField] float aggroAtMaxDistance;
    [SerializeField] float aggroOffset;
    [SerializeField] float aggroThreshold;

    [SerializeField] Animator animator;

    GameObject player;

    EnemyMove enemy;

    Enemy enemyHealth;

    float playerAggro;

    public GameObject target { get; private set; }

    private void Start()
    {
        GetComponents();

        SetValues();
    }

    void GetComponents()
    {
        player = GameObject.Find("Player");
        enemy = GetComponent<EnemyMove>();
        enemyHealth = GetComponent<Enemy>();
    }

    void SetValues()
    {
        bazooka.weaponPivot = bazooka.transform.parent.transform;
        bazooka.shotCD = bazookaShotCD;
        laser.weaponPivot = laser.transform.parent.transform;
        laser.shotCD = laserCD;
    }

    protected override void UpdateWithPause()
    {
        RotateAndShoot();
    }

    void RotateAndShoot()
    {
        target = Targeting();

        bazooka.target = this.target;

        if (enemy.currentState != enemy.stunnedState)
        {
            Aim(target, bazooka);

            Aim(target, laser);
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

    void Aim(GameObject target, EnemyWeapon weapon)
    {
        float distanceToTarget = Vector3.Distance(weapon.transform.position, target.transform.position);
        weapon.timeSinceLastShot -= Time.deltaTime;

        float t = Mathf.Clamp01((distanceToTarget - minDistanceAim) / (maxDistanceAim - minDistanceAim));
        float marginOfError = Mathf.Lerp(minValue, maxValue, t);

        Vector3 targetVelocity = target.GetComponent<Rigidbody>().velocity;
        Vector3 predictedTargetPosition = target.transform.position + targetVelocity * (distanceToTarget / weapon.projectileSpeed) * marginOfError;

        Vector3 direction = predictedTargetPosition - weapon.transform.position;

        if (direction.magnitude > Mathf.Epsilon && direction.magnitude < Mathf.Infinity)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

            float rotationSpeed = Mathf.Clamp(angleDifference / maxRotationTime, minRotationSpeed, maxRotationSpeed);

            float currentToTargetRotation = Compare(targetRotation, transform.rotation);

            if (currentToTargetRotation < 50)
            {
                weapon.weaponPivot.transform.rotation = Quaternion.Slerp(weapon.weaponPivot.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 0.2f * Time.deltaTime);

            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

            if (distanceToTarget < range && weapon.timeSinceLastShot < 0.0f)
            {
                if (weapon == bazooka)
                {
                    animator.SetTrigger("ShootRocket");
                    weapon.timeSinceLastShot = weapon.shotCD;
                }
                else
                {
                    weapon.Shoot();
                    weapon.timeSinceLastShot = weapon.shotCD;
                }
            }
        }
    }

    public void FireRocket()
    {
        Debug.Log("pog");
    }

    private float Compare(Quaternion quatA, Quaternion quatB)
    {
        return Quaternion.Angle(quatA, quatB);
    }
}
