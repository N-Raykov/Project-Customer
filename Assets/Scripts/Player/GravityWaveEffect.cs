using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWaveEffect : MonoBehaviourWithPause
{
    public float stunDuration;
    public float floatHeight;
    public float floatSpeed;
    public bool isActive = true;
    EnemyMove enemy;
    List<EnemyMove> enemiesHit;

    private void Awake()
    {
        enemiesHit = new List<EnemyMove>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        enemy = collision.gameObject.GetComponent<EnemyMove>();


        if(enemy != null && isActive == true)
        {
            enemiesHit.Add(enemy);
            enemy.GetStunned(stunDuration + 999);
        }
    }

    private void Update()
    {
        foreach(EnemyMove enemy in enemiesHit)
        {
            if (Time.time > enemy.stunDuration - 999)
            {
                enemy.GetComponent<Rigidbody>().useGravity = true;
                enemiesHit.Remove(enemy);
            }
            else
            {
                enemy.GetComponent<Rigidbody>().useGravity = false;
                enemy.agent.enabled = false;
                enemy.transform.position = Vector3.Lerp(enemy.transform.position, new Vector3(enemy.agent.transform.position.x, enemy.agent.transform.position.y + floatHeight, enemy.agent.transform.position.z), floatSpeed * Time.deltaTime);
            }
        }
    }

}
