using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDegradationTest : MonoBehaviour
{

    private MeshRenderer meshRenderer;

    //We're using a "MaterialPropertyBlock" to change only a property on THIS specific instance
    private MaterialPropertyBlock leavesDegradationMat;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        //leavesDegradationMat = meshRenderer.material;
        leavesDegradationMat = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(leavesDegradationMat);
    }

    // Update is called once per frame
    void Update()
    {
        leavesDegradationMat.SetFloat("_DegradationProgress", Mathf.Sin(Time.realtimeSinceStartup) + 0.5f);
        meshRenderer.SetPropertyBlock(leavesDegradationMat);
    }
}
