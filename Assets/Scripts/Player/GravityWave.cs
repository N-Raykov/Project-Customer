using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWave : PlayerAbility
{
    [SerializeField] float expandSpeed;
    [SerializeField] float maxScale;
    [SerializeField] float stunDuration;
    [SerializeField] float floatingHeight;
    [SerializeField] float floatSpeed;
    [SerializeField] ParticleSystem abilityAnimation;

    protected override void UseAbility()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.GetComponent<Renderer>().enabled = false; 
        sphere.transform.position = transform.position;

        abilityAnimation.Play();

        Collider sphereCollider = sphere.GetComponent<Collider>();
        if (sphereCollider != null)
        {
            sphereCollider.isTrigger = true;
        }
        sphere.AddComponent<GravityWaveEffect>();
        sphere.GetComponent<GravityWaveEffect>().stunDuration = stunDuration;
        sphere.GetComponent<GravityWaveEffect>().floatHeight = floatingHeight;
        sphere.GetComponent<GravityWaveEffect>().floatSpeed = floatSpeed;

        StartCoroutine(ExpandAndDestroy(sphere));
    }

    private IEnumerator ExpandAndDestroy(GameObject sphere)
    {
        float currentScale = 0.1f;
        Vector3 originalScale = sphere.transform.localScale;

        while (currentScale < maxScale)
        {
            currentScale += expandSpeed * Time.deltaTime;
            sphere.transform.localScale = originalScale * currentScale;
            yield return null;

        }

        if (currentScale >= maxScale)
        {
            sphere.GetComponent<GravityWaveEffect>().isActive = false;
            yield return new WaitForSeconds(5);
            Destroy(sphere);
        }
    }
}
