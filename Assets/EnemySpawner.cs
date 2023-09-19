using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviourWithPause{

    [Header("Data")]
    [SerializeField] GameObject enemy;
    [SerializeField] List<Zone> zones;
    [SerializeField] GoodPlayerControls player;

    [Header("RandomSpawns")]
    [SerializeField] float minSpawnTimeRandom;
    [SerializeField] float maxSpawnTimeRandom;




    GameObject[] bigTreesTemp;
    List<Tree> bigTrees;

    private void Start(){
        bigTrees = new List<Tree>();
        bigTreesTemp = GameObject.FindGameObjectsWithTag("BigTree");
        foreach (GameObject g in bigTreesTemp) {
            bigTrees.Add(g.GetComponent<Tree>());
        }
        bigTreesTemp = new GameObject[0];

        //Debug.Log(bigTrees.Count);
    }

    protected override void UpdateWithPause() {
        


        for (int i = 0; i < bigTrees.Count; i++) {
            if (bigTrees[i].hasFallen) {
                //SpawnWave();
                bigTrees.RemoveAt(i);
                i--;
            }
        }
    }

    void SpawnWave() {
        Debug.Log("you gonna die "+ player.zone);
    }

    void Spawn(){
        float enemyHeight = enemy.GetComponent<NavMeshAgent>().height;

        Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y + enemyHeight, transform.position.z);

        Instantiate(enemy, spawnPoint, Quaternion.identity);
    }
}