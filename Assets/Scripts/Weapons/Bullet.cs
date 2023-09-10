using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourWithPause{

    Rigidbody rb;
    [SerializeField] Transform pivot;
    [SerializeField] float speed;
    public float damage { get; set; }
    Vector3 startPosition;
    int range = 200;

    void Awake(){
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    protected override void UpdateWithPause(){
        if ((transform.position - startPosition).magnitude > range) {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision){
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        Tree tree = collision.gameObject.GetComponent<Tree>();
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (enemy != null) {
            enemy.TakeDamage(damage);
        }
        if (tree != null) {
            tree.TakeDamage(collision.contacts[0].normal);
        }
        if (player != null){
            player.TakeDamage(damage);
        }
        Destroy(this.gameObject);
    }

    public void AddSpeed(Vector3 direction) {
        if (rb.velocity.magnitude == 0) {
            rb.AddForce(direction * speed, ForceMode.Impulse);
        }
    }
}
