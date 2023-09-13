using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeTreeInteraction : MonoBehaviourWithPause{

    [SerializeField] Animator animator;
    bool isIdle = true;
    float timeBecameNonIdle;
    float timeReverseTime;
    //float swingDuration;
    float timePassed;
    

    protected override void UpdateWithPause() {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);


        if (!state.IsName("Idle")){
            if (isIdle){
                isIdle = false;
                timeBecameNonIdle = Time.time;
            }
            //swingDuration = state.length;
            //Debug.Log(swingDuration);

        }
        else {
            isIdle = true;
            animator.SetFloat("speed", 1);
        }


        if (!isIdle && timePassed != 0 && Time.time - timeReverseTime > timePassed) {
            Debug.Log("working");
            timePassed = 0;
            animator.SetTrigger("Reverse");

        }

        //Debug.Log(state.normalizedTime);

        //Debug.Log(state.length);
        //if (state.IsName("Idle"))
        //{
        //    //animator.SetFloat("speed", 1);
        //    //Debug.Log(1);
        //}
        //else
        //{
        //    if (isIdle)
        //    {
        //        isIdle = false;
        //        timeBecameNonIdle = Time.time;
        //    }

        //    if (Time.time - timeSpeedChanged > timeSpeedChanged - timeBecameNonIdle)
        //    {

        //        //animator.
        //        isIdle = true;


        //    }
        //}


    }


    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Log") && !isIdle) {
            animator.SetFloat("speed", -1);
            //timeSpeedChanged = Time.time;
            timePassed = (Time.time - timeBecameNonIdle);
            timeReverseTime = Time.time;
            //animator.SetFloat("offset", timeAnimation); // (Time.time - timeBecameNonIdle)); // swingDuration - 
            //Debug.Log(timeAnimation);
            //animator.SetTrigger("Reverse");

            //Debug.LogError("q");

        }

    }
    
}
