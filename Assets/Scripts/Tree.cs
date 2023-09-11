using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviourWithPause {

    [SerializeField] int hp;
    [SerializeField] int value;
    Rigidbody rb;
    public bool hasFallen { get; private set; }
    public int _value{ get; private set; }

    void Start(){
        _value = value;
        rb = GetComponent<Rigidbody>();
        hasFallen = false;
    }

    public void TakeDamage(Vector3 pNormal) {
        hp--;
        if (hp == 0) {
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(pNormal*35,ForceMode.Force);//was 30
        }
    }

    private void OnCollisionStay(Collision collision){
        if (collision.gameObject.CompareTag("Ground")) {
            hasFallen = true;
            //normal = collision.contacts[0].normal;
            //Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point+10*normal);
        }
    }

    //public void StartTakeOff() {

    //}

}
