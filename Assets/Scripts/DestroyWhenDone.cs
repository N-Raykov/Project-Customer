using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenDone : MonoBehaviour{
    ParticleSystem particles;

    private void Start(){
        particles=GetComponent<ParticleSystem>();
    }
    void Update(){
        if (particles != null && !particles.isPlaying)
            Destroy(gameObject);
    }

}
