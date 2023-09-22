using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun{

    [Header("Sounds")]
    [SerializeField] AudioSource gunshot;
    [SerializeField] AudioClip gunshotSound;
    [SerializeField] AudioClip bulletInsert;
    [SerializeField] AudioClip rack;

    private void Awake(){
        canBeAccessed = true;
        gameObject.SetActive(false);
    }

    protected override void CreateBullet(){
        for (int i = 0; i < 8; i++){
            GameObject b = Instantiate(bullet, muzzle.position, mainCamera.transform.rotation);
            Bullet bt = b.GetComponent<Bullet>();
            bt.damage = gunData.damage;
            bt.speed = gunData.bulletSpeed;
            bt.range = gunData.range;
            bt.AddSpeed(AimAtTarget());

        }
        Instantiate(muzzleFlash, muzzle.position, mainCamera.transform.rotation, muzzle);
    }

    protected override void StartReloadAnimation(){
        //Debug.Log("reload");
        animator.SetTrigger("Reload");
        //throw new System.NotImplementedException();
    }
    protected override void StartShotAnimation()
    {
        animator.SetTrigger("Shoot");
        gunshot.PlayOneShot(gunshotSound);
        //throw new System.NotImplementedException();
    }

    void PlayBulletInsert()
    {
        gunshot.PlayOneShot(bulletInsert);
    }

    void PlayRack()
    {
        gunshot.PlayOneShot(rack);
    }

}
