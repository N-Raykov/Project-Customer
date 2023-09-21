using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeTreeInteraction : MonoBehaviourWithPause{

    [Header("Data")]
    [SerializeField] Transform cameraPivot;
    [SerializeField] Animator animator;
    [SerializeField] GunData data;

    bool isIdle = true;
    float timeBecameNonIdle;
    float timeReverseTime;
    float timePassed;
    

    protected override void UpdateWithPause() {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);


        if (!state.IsName("Idle")){
            if (isIdle){
                isIdle = false;
                timeBecameNonIdle = Time.time;
            }

        }
        else {
            isIdle = true;
            animator.SetFloat("speed", 1);
        }


        if (!isIdle && timePassed != 0 && Time.time - timeReverseTime > timePassed) {
            timePassed = 0;
            animator.SetTrigger("Reverse");

        }

    }

    private void OnTriggerEnter(Collider other){

        Debug.Log(other.tag);

        if (other.CompareTag("Log") && !isIdle) {
            Debug.Log(1);
            other.GetComponent<Tree>().TakeDamage(transform.right);
            //animator.SetFloat("speed", -1);
            //timePassed = (Time.time - timeBecameNonIdle);
            //timeReverseTime = Time.time;
        }

        if ((other.CompareTag("BigTree")|| (other.CompareTag("Stump")) && !isIdle)){
            animator.SetFloat("speed", -1);
            timePassed = (Time.time - timeBecameNonIdle);
            timeReverseTime = Time.time;
        }

        if (other.CompareTag("Enemy") && !isIdle){
            other.GetComponent<Enemy>().TakeDamage(data.damage);
            animator.SetFloat("speed", -1);
            timePassed = (Time.time - timeBecameNonIdle);
            timeReverseTime = Time.time;
        }
    }
    
}
