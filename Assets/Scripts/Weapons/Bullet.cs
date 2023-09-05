using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] Transform pivot;
    [SerializeField] float speed;
    Vector3 startPosition;
    int range = 200;

    public int number;

    void Awake(){
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        startPosition = transform.position;
    }

    private void Update(){
        if ((transform.position - startPosition).magnitude > range) {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "Target") {
            //collision.gameObject.GetComponent<TargetBehavior>().HitBehavior();
        }
        Destroy(this.gameObject);
        
    }

    public void AddSpeed(Vector3 direction) {
        if (rb.velocity.magnitude == 0) {
            rb.AddForce(direction * speed, ForceMode.Impulse);
        }
    }
}
