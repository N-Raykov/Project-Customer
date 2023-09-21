using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckForDropboxes : MonoBehaviourWithPause{
    [SerializeField] ShopManager shopManager;
    [SerializeField] Material green;
    [SerializeField] Material red;
    public bool canBeSpawnedOn = true;
    void Start(){
        ignorePausedState = true;
        GetComponent<Renderer>().material = green;
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Renderer>().material = red;
        canBeSpawnedOn = false;
        //shopManager.RemoveFromSpawnpoints(transform.localPosition);

    }
    private void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material = green;
        canBeSpawnedOn = true;
        //shopManager.AddtoSpawnpoints(transform.localPosition);
    }

}
