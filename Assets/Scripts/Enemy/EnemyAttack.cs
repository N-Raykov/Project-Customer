using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviourWithPause
{
    [SerializeField] GameObject player;
    [SerializeField] EnemyGun gun;
    [SerializeField] float range;
    [SerializeField] float shotCD;
    float timeSinceLastShot;

    protected override void UpdateWithPause()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        timeSinceLastShot -= Time.deltaTime;
        if (distanceToPlayer < range && timeSinceLastShot < 0.0f)
        {
            gun.Shoot();
            timeSinceLastShot = shotCD;
        }
    }
}
