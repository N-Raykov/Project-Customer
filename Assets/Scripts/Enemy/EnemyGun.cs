using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public abstract class EnemyGun : EnemyWeapon
{
    protected float lastShotTime = 0;

    [Header("Data")]
    [SerializeField] protected Animator animator;

    [Header("Objects")]
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected GameObject muzzleFlash;

    private void Start()
    {
        projectileSpeed = gunData.bulletSpeed;
    }

    public override void Shoot()
    {
        StartShotAnimation();
        lastShotTime = Time.time;
        projectileSpeed = gunData.bulletSpeed;
        CreateBullet();
    }

    protected virtual void CreateBullet()
    {
        GameObject b = Instantiate(bullet, muzzle.position, pivot.rotation);
        Bullet bt = b.GetComponent<Bullet>();
        bt.damage = gunData.damage;
        bt.speed = gunData.bulletSpeed;
        bt.range = gunData.range;
        bt.AddSpeed(AimAtTarget());
        Instantiate(muzzleFlash, muzzle.position, pivot.transform.rotation, muzzle);
    }

    protected abstract void StartShotAnimation();

    protected abstract void StartReloadAnimation();


}
