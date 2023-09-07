using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPod : MonoBehaviour{
    Rigidbody rb;
    public ShopButtonData data { get; set; }
    void Start(){
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.down*1000,ForceMode.Force);
    }

    void Update(){
        
    }
}
