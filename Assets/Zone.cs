using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviourWithPause {

    [SerializeField] int waveSize;
    public int _waveSize { get; private set; }
    public int zoneNumber { get; set; }

    private void Awake(){
        _waveSize = waveSize;
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.GetComponent<GoodPlayerControls>()) {
            other.gameObject.GetComponent<GoodPlayerControls>().zone = zoneNumber;
        }
    }

}
