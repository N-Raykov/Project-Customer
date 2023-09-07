using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour{

    [SerializeField] int hp;
    Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }
    public void TakeDamage(Vector3 pNormal) {
        hp--;
        if (hp == 0) {
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(pNormal*30,ForceMode.Force);
            //rb.AddTorque()
        }
    }

}
