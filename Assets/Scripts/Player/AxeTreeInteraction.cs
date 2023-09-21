using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeTreeInteraction : MonoBehaviourWithPause{

    [Header("Data")]
    [SerializeField] Transform cameraPivot;
    [SerializeField] Animator animator;
    [SerializeField] GunData data;
    [SerializeField] GameObject splinters;

    [Header("Sounds")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip sound1;
    [SerializeField] AudioClip sound2;
    [SerializeField] AudioClip sound3;

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
            Instantiate(splinters,transform.position,Quaternion.identity);
            Debug.Log(1);
            other.GetComponent<Tree>().TakeDamage(transform.right);
            PlaySound();
            //animator.SetFloat("speed", -1);
            //timePassed = (Time.time - timeBecameNonIdle);
            //timeReverseTime = Time.time;
        }

        if ((other.CompareTag("BigTree")|| (other.CompareTag("Stump")) && !isIdle)){
            Instantiate(splinters, transform.position, Quaternion.identity);
            PlaySound();
            //animator.SetFloat("speed", -1);
            //timePassed = (Time.time - timeBecameNonIdle);
            //timeReverseTime = Time.time;
        }

        if (other.CompareTag("Enemy") && !isIdle){
            Instantiate(splinters, transform.position, Quaternion.identity);
            other.GetComponent<Enemy>().TakeDamage(data.damage);
            //animator.SetFloat("speed", -1);
            //timePassed = (Time.time - timeBecameNonIdle);
            //timeReverseTime = Time.time;
        }
    }

    void PlaySound() {
        int randomNumber = UnityEngine.Random.Range(0, 3);
        switch (randomNumber) {
            case 0:
                source.PlayOneShot(sound1);
                break;
            case 1:
                source.PlayOneShot(sound2);
                break;
            case 2:
                source.PlayOneShot(sound3);
                break;
        }
    }
    
}
