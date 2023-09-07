using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public float stunDuration;
    
    private void OnTriggerEnter(Collider collision)
    {
        EnemyMove enemy = collision.gameObject.GetComponent<EnemyMove>();
        if(enemy != null)
        {
            enemy.GetStunned(stunDuration);
        }
    }
}
