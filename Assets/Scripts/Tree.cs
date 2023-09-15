using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviourWithPause {

    [SerializeField] int hp;
    [SerializeField] int value;
    [SerializeField] float pushForce;
    [SerializeField] float duration;
    [SerializeField] float timeDelay;
    [SerializeField] float directionDivisioFactor;
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
            rb.AddForce(pNormal*pushForce,ForceMode.Force);//was 35
            StartCoroutine(Move(pNormal));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"collision {collision.gameObject.name}");
    }

    private void OnCollisionStay(Collision collision){
        if (collision.gameObject.CompareTag("Ground") && hasFallen == false) {
            hasFallen = true;
            GameManager.fallenTrees++;
            Debug.Log("tree has fallen");
            //normal = collision.contacts[0].normal;
            //Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point+10*normal);
        }
    }

    IEnumerator Move(Vector3 pNormal) {

        int i = 0;
        while (i < duration) {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.AddForce(pNormal/directionDivisioFactor, ForceMode.VelocityChange);
            rb.constraints = RigidbodyConstraints.None;
            i++;
            yield return new WaitForSeconds(timeDelay);//0.125

            //rb.constraints = RigidbodyConstraints.FreezeRotation;
            //rb.AddForce(pNormal / 1.5f, ForceMode.VelocityChange);
            //Debug.Log("guatafac amigos");
            //rb.constraints = RigidbodyConstraints.None;
            //i++;
            //yield return new WaitForSeconds(0.25f);
        }
    }

    //public void StartTakeOff() {

    //}

}
