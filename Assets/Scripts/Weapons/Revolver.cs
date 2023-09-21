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

    private void Awake(){
        canBeAccessed = true;
        gameObject.SetActive(false);
    }

    protected override void Start()
    {
        recoilTargetRotation = Vector3.zero;
        state = States.Idle;
        animator = transform.GetComponent<Animator>();
        currentAmmo = 0;
        InvokeOnAmmoChange();
        pistolRotationPivotStartPosition = pistolRotationPivot.localPosition;
        originalFOV = mainCamera.fieldOfView;
        originalFOVWeaponCamera = weaponCamera.fieldOfView;
    }

    protected override void StartShotAnimation(){
        animator.SetTrigger("Shoot");
        //StartCoroutine(SpinMag());
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
    protected override void CreateBullet(){
        Vector3 targetRotation = mainCamera.transform.eulerAngles - new Vector3(0,90,0);
        GameObject b = Instantiate(bullet, muzzle.position, Quaternion.Euler(targetRotation));
        Bullet bt = b.GetComponent<Bullet>();
        bt.damage = gunData.damage;
        bt.speed = gunData.bulletSpeed;
        bt.range = gunData.range;
        bt.AddSpeed(AimAtTarget());
        Instantiate(muzzleFlash, muzzle.position, mainCamera.transform.rotation, muzzle);
    }

    void PlayCylinderStart(){
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
