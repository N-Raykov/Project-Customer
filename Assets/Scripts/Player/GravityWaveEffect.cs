using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWaveEffect : MonoBehaviourWithPause
{
    public float stunDuration;
    public float floatHeight;
    EnemyMove enemy;

    private void OnTriggerEnter(Collider collision)
    {
        enemy = collision.gameObject.GetComponent<EnemyMove>();

        if(enemy != null)
        {
            enemy.GetGravityGloved(stunDuration, floatHeight);
            Debug.Log(stunDuration + " " + floatHeight);
        }
    }

}
