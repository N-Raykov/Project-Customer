using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviourWithPause {

    [SerializeField] List<Transform> points;
    public Vector3[] spawnPoints { get; private set; }
    public List<Vector3> freeSpawnPoints { get;  set; }
    public int zoneNumber { get; set; }

    public IEnumerator MakeSpawnPointFreeAfterTime(float pTime,Vector3 pSpawnPoint) {
        yield return new WaitForSeconds(pTime);
        freeSpawnPoints.Add(pSpawnPoint);
    }

    private void Awake(){
        spawnPoints = new Vector3[20];
        freeSpawnPoints = new List<Vector3>();
        int i = 0;
        foreach (Transform t in points) {
            spawnPoints[i]=(t.position);
            freeSpawnPoints.Add(spawnPoints[i]);
        }
    }

    private void OnTriggerStay(Collider other){
        if (other.gameObject.GetComponent<GoodPlayerControls>()) {
            other.gameObject.GetComponent<GoodPlayerControls>().zone = zoneNumber;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<GoodPlayerControls>()){
            other.gameObject.GetComponent<GoodPlayerControls>().zone = -1;
        }
    }

}
