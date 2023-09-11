using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenDone : MonoBehaviourWithPause{
    ParticleSystem particles;

    private void Start(){
        particles=GetComponent<ParticleSystem>();
    }
    protected override void UpdateWithPause(){
        if (particles != null && !particles.isPlaying)
            Destroy(gameObject);
    }

}
