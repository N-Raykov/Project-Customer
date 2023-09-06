using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] Transform pivot;
    [SerializeField] float speed;
    [SerializeField] float damage;
    Vector3 startPosition;
    int range = 200;

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
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);
        }
        Destroy(this.gameObject);
    }

    public void AddSpeed(Vector3 direction) {
        if (rb.velocity.magnitude == 0) {
            rb.AddForce(direction * speed, ForceMode.Impulse);
        }
    }
}
