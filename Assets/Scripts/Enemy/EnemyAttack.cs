using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] EnemyGun gun;
    [SerializeField] float range;
    [SerializeField] float shotCD;
    float timeSinceLastShot;

    private void Update()
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
