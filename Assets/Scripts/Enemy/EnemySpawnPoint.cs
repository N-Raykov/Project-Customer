using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnPoint : MonoBehaviourWithPause
{
    [SerializeField] List<int> treeRequirements = new List<int>();
    private HashSet<int> requirementsMet = new HashSet<int>();

    [SerializeField] GameObject enemy;

    protected override void UpdateWithPause()
    {
        foreach (var requirement in treeRequirements)
        {
            if (GameManager.fallenTrees >= requirement && !requirementsMet.Contains(requirement))
            {
                requirementsMet.Add(requirement);
                Spawn();
            }
        }
    }

    void Spawn()
    {
        float enemyHeight = enemy.GetComponent<NavMeshAgent>().height;

        Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y + enemyHeight, transform.position.z);

        Instantiate(enemy, spawnPoint, Quaternion.identity);
    }
}
