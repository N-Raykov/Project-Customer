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

    GameObject player;

    [SerializeField] EnemyMove enemy;

    float playerAggro;

    private void Start()
    {
        gunPivot = gun.transform.parent.transform;
        player = GameObject.Find("Player");
    }

    protected override void UpdateWithPause()
    {
        RotateAndShoot();
    }

    void RotateAndShoot()
    {
        GameObject target = Targeting();

        Aim(target);
    }


    GameObject Targeting()
    {
        GameObject target;

        if (GameManager.robot != null && playerAggro < 100)
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
        float maxValue = 1f;

        float t = Mathf.Clamp01((distanceToTarget - minDistance) / (maxDistance - minDistance));
        float marginOfError = Mathf.Lerp(minValue, maxValue, t);

        Vector3 targetVelocity = target.GetComponent<Rigidbody>().velocity;
        Vector3 predictedTargetPosition = target.transform.position + targetVelocity * (distanceToTarget / gun.projectileSpeed) * marginOfError;

        Vector3 direction = predictedTargetPosition - gun.transform.position;

        Debug.Log(targetVelocity);


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
