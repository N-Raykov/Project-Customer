using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObjectTimer : MonoBehaviour
{

    [SerializeField] List<MeshRenderer> meshes;
    [SerializeField] Color targetColor;
    [SerializeField] float Speed = 1.5f;
    private bool isHighlighting = false;

    void EndHighlightTimer()
    {
        isHighlighting = false;

        foreach (var meshrenderer in meshes)
        {
            foreach (var material in meshrenderer.materials)
            {
                material.DisableKeyword("_EMISSION");
                material.SetColor("__EmissiveColor", Color.black);
            }
        }
    }

    public void StartHighlightTimer(float duration)
    {
        isHighlighting = true;
        Invoke("EndHighlightTimer", duration);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHighlighting)
        {
            foreach (var meshrenderer in meshes)
            {
                for (int i = 0; i < meshrenderer.materials.Length; i++)
                {
                    Material material = meshrenderer.materials[i];
                    //material.EnableKeyword("")
                    material.EnableKeyword("_EMISSION");
                    material.SetColor("_EmissionColor", Color.Lerp(Color.black, targetColor, (Mathf.Sin(Time.time * Speed) /2) + 0.5f ));
                    //meshrenderer.materials[i] = material;
                }
            }
        }
    }
}
