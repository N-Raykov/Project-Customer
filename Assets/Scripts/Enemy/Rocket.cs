using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviourWithPause
{
    Rigidbody rb;
    public float damage { get; set; }
    public float range { get; set; }
    public float speed { get; set; }
    public GameObject target { get; set; }

    Vector3 startPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    protected override void UpdateWithPause()
    {
        if ((transform.position - startPosition).magnitude > range)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        //if (tree != null) {
        //    tree.TakeDamage(collision.contacts[0].normal);
        //}
        if (player != null)
        {
            player.TakeDamage(damage);
        }
        Destroy(this.gameObject);
    }

    public void AddSpeed(Vector3 direction)
    {
        if (rb.velocity.magnitude == 0)
        {
            rb.AddForce(direction * speed, ForceMode.Impulse);
        }
    }
}
