using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPod : MonoBehaviourWithPause{
    Rigidbody rb;
    public string item { get; set; }
    public int amount { get; set; }

    void Start(){
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.down*1000,ForceMode.Force);
    }

    void Update(){
        
    }
}
