using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [System.NonSerialized] public float projectileSpeed;

    [Header("Data")]
    [SerializeField] protected GunData gunData;

    [Header("Objects")]
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected Transform pivot;

    [System.NonSerialized] public float timeSinceLastShot;
    [System.NonSerialized] public float shotCD;
    [System.NonSerialized] public Transform weaponPivot;

    public virtual void Shoot()
    {

    }

    protected Vector3 AimAtTarget()
    {
        RaycastHit info;
        Vector3 targetPosition;

        if (Physics.Raycast(pivot.position, pivot.forward, out info, gunData.range))
            targetPosition = info.point;
        else
            targetPosition = pivot.position + pivot.forward * gunData.range;

        targetPosition += new Vector3(UnityEngine.Random.Range(-gunData.spreadFactorX, gunData.spreadFactorX), UnityEngine.Random.Range(-gunData.spreadFactorY, gunData.spreadFactorY), 0);

        return (targetPosition - muzzle.position).normalized;
    }
}
