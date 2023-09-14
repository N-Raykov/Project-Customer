using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBazooka : EnemyWeapon
{
    protected float lastShotTime = 0;

    [Header("Objects")]
    [SerializeField] protected GameObject rocket;

    private void Start()
    {
        projectileSpeed = gunData.bulletSpeed;
    }

    public override void Shoot()
    {
        lastShotTime = Time.time;
        projectileSpeed = gunData.bulletSpeed;
        CreateBullet();
    }

    protected virtual void CreateBullet()
    {
        GameObject b = Instantiate(rocket, muzzle.position, pivot.rotation);
        Bullet bt = b.GetComponent<Bullet>();
        bt.damage = gunData.damage;
        bt.speed = gunData.bulletSpeed;
        bt.range = gunData.range;
        bt.AddSpeed(AimAtTarget());
    }

}
