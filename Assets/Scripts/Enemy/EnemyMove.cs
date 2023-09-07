using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviourWithPause
{

    [SerializeField] GameObject player;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void UpdateWithPause()
    {
        Vector3 toPlayer = player.transform.position - transform.position;
        if (Vector3.Distance(player.transform.position, transform.position) < 3)
        {
            Vector3 targetPosition = toPlayer.normalized * -3f;
            agent.destination = targetPosition;
        }

        agent.destination = player.transform.position;
        transform.LookAt(player.transform);
    }
}
