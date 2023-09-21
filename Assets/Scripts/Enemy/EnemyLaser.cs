using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : EnemyWeapon
{
    protected float lastShotTime = 0;

    [Header("Objects")]
    [SerializeField] protected GameObject laser;
    public GameObject target { get; set; }

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
        GameObject l = Instantiate(laser, muzzle.position, pivot.rotation);
        Bullet lzr = l.GetComponent<Bullet>();
        lzr.damage = gunData.damage;
        lzr.speed = gunData.bulletSpeed;
        lzr.range = gunData.range;
        lzr.AddSpeed(transform.forward);
    }

}
