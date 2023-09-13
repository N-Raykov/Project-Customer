using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COG : MonoBehaviour
{
    [SerializeField] Transform centerOfGravity;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfGravity.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
