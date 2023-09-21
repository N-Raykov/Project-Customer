using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckForDropboxes : MonoBehaviourWithPause{
    [SerializeField] Material green;
    [SerializeField] Material red;
    void Start(){
        ignorePausedState = true;
        GetComponent<Renderer>().material = green;
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Renderer>().material = red;
    }
    private void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material = green;
    }

}
