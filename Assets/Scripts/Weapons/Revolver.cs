using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Gun{

    [Header("Mag")]
    [SerializeField] Transform mag;

    [Header ("Sounds")]
    [SerializeField] AudioSource gunshot;
    [SerializeField] AudioClip gunshotSound;
    [SerializeField] AudioClip cylinderSoundStart;
    [SerializeField] AudioClip cylinderSoundEnd;
    [SerializeField] AudioClip bulletFallingSound;

    protected override void StartShotAnimation(){
        animator.SetTrigger("Shoot");
        StartCoroutine(SpinMag());
        gunshot.PlayOneShot(gunshotSound);
    }

    protected override void StartReloadAnimation() {
        animator.SetTrigger("Reload");
    }

    IEnumerator SpinMag(){
        int i = 0;
        while (i < 20){
            i++;
            mag.eulerAngles = new Vector3(mag.eulerAngles.x, mag.eulerAngles.y, mag.eulerAngles.z + 6);//should be +3 to be realistic but it looks better like this
            yield return new WaitForSeconds(0.01f);
        }
    }

    void PlayCylinderStart()
    {
        gunshot.PlayOneShot(cylinderSoundStart);
    }

    void PlayBulletFallingSound()
    {
        gunshot.PlayOneShot(bulletFallingSound);
    }
    void PlayCylinderEnd()
    {
        gunshot.PlayOneShot(cylinderSoundEnd);
    }

}
