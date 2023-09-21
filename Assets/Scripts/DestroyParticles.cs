using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class DestroyParticles : MonoBehaviour
{
    VisualEffect vfx;

    private void Start()
    {
        vfx = GetComponent<VisualEffect>();
    }

    private bool removing = false;
    private float timeElapsed = 0f; 

    private void Update()
    {
        int particleCount = vfx.aliveParticleCount;

        if (particleCount == 0)
        {
            if (!removing)
            {
                removing = true;
                timeElapsed = 0f;
            }
            else
            {
                timeElapsed += Time.deltaTime;

                if (timeElapsed >= 3f)
                {
                    Remove();
                }
            }
        }
        else
        {
            removing = false;
            timeElapsed = 0f;
        }

    }
    private void Remove()
    {
        Destroy(this.gameObject);
    }
}
