using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEMP : PlayerAbility
{
    [SerializeField] float expandSpeed;
    [SerializeField] float maxScale;
    [SerializeField] float stunDuration;

    protected override void UseAbility()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.GetComponent<Renderer>().enabled = true; 
        sphere.transform.position = transform.position;

        Collider sphereCollider = sphere.GetComponent<Collider>();
        if (sphereCollider != null)
        {
            sphereCollider.isTrigger = true;
        }
        sphere.AddComponent<CollisionDetection>();
        sphere.GetComponent<CollisionDetection>().stunDuration = stunDuration;

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

        Destroy(sphere);
    }
}
