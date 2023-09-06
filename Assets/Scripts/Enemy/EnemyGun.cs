using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public abstract class EnemyGun : MonoBehaviour
{
    protected enum States
    {
        Idle,
        Shoot,
        Reload
    }
    protected States state = States.Idle;
    protected float lastShotTime = 0;

    [Header("Data")]
    [SerializeField] protected GunData gunData;
    [SerializeField] protected Animator animator;

    [Header("Objects")]
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected Transform pivot;
    [SerializeField] protected Transform pistolRotationPivot;
    [SerializeField] protected GameObject muzzleFlash;


    public void Shoot()
    {
        StartShotAnimation();
        lastShotTime = Time.time;
        GameObject b = Instantiate(bullet, muzzle.position, pivot.rotation);
        b.GetComponent<Bullet>().AddSpeed(AimAtTarget());
        Instantiate(muzzleFlash, muzzle.position, pivot.rotation, muzzle);
        state = States.Shoot;
    }

    protected abstract void StartShotAnimation();

    protected abstract void StartReloadAnimation();

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
