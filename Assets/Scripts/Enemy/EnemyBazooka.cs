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
        GameObject r = Instantiate(rocket, muzzle.position, pivot.rotation);
        Rocket rckt = r.GetComponent<Rocket>();
        rckt.damage = gunData.damage;
        rckt.speed = gunData.bulletSpeed;
        rckt.range = gunData.range;
        rckt.AddSpeed(transform.forward);
    }

}
