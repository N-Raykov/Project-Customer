using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDegradationTest : MonoBehaviour
{

    private MeshRenderer meshRenderer;
    private Material leavesDegradationMat;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        leavesDegradationMat = meshRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        leavesDegradationMat.SetFloat("_DegradationProgress", Mathf.Sin(Time.realtimeSinceStartup) + 0.5f);
    }
}
