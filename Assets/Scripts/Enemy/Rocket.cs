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

    [SerializeField] float homingRange;
    [SerializeField] float homingStrength;
    bool canHome = true;

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

        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget > homingRange && canHome)
            {
                FollowTarget(target);
            }
            else
            {
                canHome = false;
            }
        }
    }

    void FollowTarget(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;

        transform.rotation = Quaternion.LookRotation(direction);

        rb.AddForce(direction * homingStrength, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        if (player != null)
        {
            player.TakeDamage(damage);
        }
        Destroy(this.gameObject);
    }
}
