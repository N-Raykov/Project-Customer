using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviourWithPause{

    [Header("Data")]
    [SerializeField] GameObject enemy;
    [SerializeField] GoodPlayerControls player;

    [Header("RandomSpawns")]
    [SerializeField] float minSpawnTimeRandom;
    [SerializeField] float maxSpawnTimeRandom;
    [SerializeField] int percentChanceForASecondEnemy;
    [SerializeField] float delayForRandomEnemy;

    [Header("Waves")]
    [SerializeField] float delayForWaves;
    [SerializeField] int waveSize;
    [SerializeField] int waveSizeIncreasePerCutTree;
    int treesCut = 0;
    int lastTreesCut = -1;


    int lastPlayerZone=0;//used for spawning waves 
    float spawnTime;
    float lastSpawnTime = -100000;
    GameObject[] zonesTemp;
    GameObject[] bigTreesTemp;
    List<Zone> zones;
    List<Tree> bigTrees;

    private void Start() {
        bigTrees = new List<Tree>();
        zones = new List<Zone>();
        zonesTemp = GameObject.FindGameObjectsWithTag("SpawnArea");
        Debug.Log(zonesTemp.Length);
        bigTreesTemp = GameObject.FindGameObjectsWithTag("BigTree");
        foreach (GameObject g in bigTreesTemp) {
            bigTrees.Add(g.GetComponent<Tree>());
        }
        int i = 0;
        foreach (GameObject g in zonesTemp) {
            zones.Add(g.GetComponent<Zone>());
            zones[i].zoneNumber = i;
            i++;
        }
        bigTreesTemp = null;
        zonesTemp = null;

    }

    protected override void UpdateWithPause() {
        //Debug.Log(player.zone);

        if (player.zone != -1) {
            lastPlayerZone = player.zone;
        }

        if (spawnTime == -1 && player.zone != -1){
            spawnTime = UnityEngine.Random.Range(minSpawnTimeRandom, maxSpawnTimeRandom);
        }

        for (int i = 0; i < bigTrees.Count; i++){
            if (bigTrees[i].hasFallen){
                //SpawnWave();
                bigTrees.RemoveAt(i);
                lastTreesCut = treesCut;
                treesCut++;
                i--;
            }
        }
        if (GameManager.robot != null) {
            Robot robot = GameManager.robot.GetComponent<Robot>();
            if (robot.currentState == Robot.RobotState.Cutting && lastTreesCut != treesCut){
                lastTreesCut++;
                SpawnWave();

            }
        }


        if (spawnTime!=-1 && Time.time - spawnTime > lastSpawnTime && player.zone!=-1) {
            lastSpawnTime = Time.time;
            spawnTime = -1;
            SpawnEnemy(delayForRandomEnemy);
            int randomNumber = UnityEngine.Random.Range(0,100/percentChanceForASecondEnemy);
            if (randomNumber == 0) {
                SpawnEnemy(delayForRandomEnemy);
            }
        }
    }

    public void SpawnEnemy(float pTime){
        if (zones[player.zone].freeSpawnPoints.Count == 0) {
            StartCoroutine(SpawnEnemyWithDelay(pTime));
            return;
        }

        int spawnPointIndice = UnityEngine.Random.Range(0,zones[player.zone].freeSpawnPoints.Count);
        zones[player.zone].StartCoroutine(zones[player.zone].MakeSpawnPointFreeAfterTime(5, zones[player.zone].freeSpawnPoints[spawnPointIndice]));
        Vector3 spawnPoint = zones[player.zone].freeSpawnPoints[spawnPointIndice];

        (Instantiate(enemy, spawnPoint, Quaternion.identity)).GetComponent<DropPod>();

        zones[player.zone].freeSpawnPoints.Remove(zones[player.zone].freeSpawnPoints[spawnPointIndice]);

    }

    IEnumerator SpawnEnemyWithDelay(float pTime) {
        yield return new WaitForSeconds(pTime);
        SpawnEnemy(pTime);
    }

    void SpawnWave() {
        Debug.Log(waveSize + treesCut * waveSizeIncreasePerCutTree);
        for (int i = 0; i < waveSize + treesCut * waveSizeIncreasePerCutTree; i++){
            SpawnEnemyForWave(delayForWaves);
        }
    }

    public void SpawnEnemyForWave(float pTime)
    {
        if (zones[lastPlayerZone].freeSpawnPoints.Count == 0)
        {
            StartCoroutine(SpawnEnemyWithDelay(pTime));
            return;
        }

        int spawnPointIndice = UnityEngine.Random.Range(0, zones[lastPlayerZone].freeSpawnPoints.Count);
        zones[lastPlayerZone].StartCoroutine(zones[lastPlayerZone].MakeSpawnPointFreeAfterTime(10, zones[lastPlayerZone].freeSpawnPoints[spawnPointIndice]));//maybe change the 10
        Vector3 spawnPoint = zones[lastPlayerZone].freeSpawnPoints[spawnPointIndice];

        (Instantiate(enemy, spawnPoint, Quaternion.identity)).GetComponent<DropPod>();

        zones[lastPlayerZone].freeSpawnPoints.Remove(zones[lastPlayerZone].freeSpawnPoints[spawnPointIndice]);

    }

}