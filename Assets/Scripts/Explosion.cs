using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] public float explosionSpeed;
    [SerializeField] public float explosionMaxSize;

    [SerializeField] float explosionDamage;

    [SerializeField] GameObject explosionParticles;

    private void Start()
    {
        Explode();
    }

    void Explode()
    {
        this.GetComponent<Renderer>().enabled = false;

        Collider sphereCollider = GetComponent<Collider>();
        if (sphereCollider != null)
        {
            sphereCollider.isTrigger = true;
        }

        Instantiate(explosionParticles, transform.position, Quaternion.identity);

        StartCoroutine(ExpandExplosion(this.gameObject));
    }

    IEnumerator ExpandExplosion(GameObject explosion)
    {
        float currentScale = 0.1f;
        Vector3 originalScale = explosion.transform.localScale;

        while (currentScale < explosionMaxSize)
        {
            currentScale += explosionSpeed * Time.deltaTime;
            explosion.transform.localScale = originalScale * currentScale;
            yield return null;

        }

        if (currentScale >= explosionMaxSize)
        {
            Destroy(explosion);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();


        if (enemy != null)
        {
            enemy.TakeDamage(explosionDamage);
        }
        if (player != null)
        {
            player.TakeDamage(explosionDamage);
        }
    }
}
