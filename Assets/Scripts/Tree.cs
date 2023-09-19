using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PARICLE TESTING, REMOVE LATER!!!
using UnityEngine.Events;

public class Tree : MonoBehaviourWithPause {

    [SerializeField] int hp;
    [SerializeField] int value;
    Rigidbody rb;
    public bool hasFallen { get; private set; }
    public int _value{ get; private set; }

    //PARICLE TESTING, REMOVE LATER!!!
    public UnityEvent dmg_event;

    void Start(){
        _value = value;
        rb = GetComponent<Rigidbody>();
        hasFallen = false;
    }

    public void TakeDamage(Vector3 pNormal) {
        hp--;

        //PARICLE TESTING, REMOVE LATER!!!
        dmg_event?.Invoke();

        if (hp == 0 && hasFallen == false) {
            hasFallen = true;
            GameManager.fallenTrees++;
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(pNormal*35,ForceMode.Force);//was 30
        }
    }

    private void OnCollisionStay(Collision collision){
        if (collision.gameObject.CompareTag("Ground") && hasFallen == false) {
            //hasFallen = true;
            //GameManager.fallenTrees++;
            Debug.Log("tree has fallen");
            //normal = collision.contacts[0].normal;
            //Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point+10*normal);
        }
    }

    //public void StartTakeOff() {

    //}

}
