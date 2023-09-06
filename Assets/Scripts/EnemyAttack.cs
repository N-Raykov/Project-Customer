using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Gun gun;
    [SerializeField] float range;
    [SerializeField] float shotCD;

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        if (distanceToPlayer < range)
        {
            gun.Shoot();
        }
    }
}
