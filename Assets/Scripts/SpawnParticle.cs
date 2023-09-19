using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnParticle : MonoBehaviour
{
    [SerializeField] private GameObject particle;
    [SerializeField] private Vector3 offset;
    
    public void InstantiateParticle()
    {
         Instantiate(particle, transform.position + offset, Quaternion.identity);
    }
}
