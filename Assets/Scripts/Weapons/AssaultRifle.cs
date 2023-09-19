using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Gun{

    private void Awake(){
        canBeAccessed = true;
        gameObject.SetActive(false);
    }
    protected override void CreateBullet(){
        Vector3 targetRotation = mainCamera.transform.eulerAngles - new Vector3(0, 90, 0);
        GameObject b = Instantiate(bullet, muzzle.position, Quaternion.Euler(targetRotation));
        Bullet bt = b.GetComponent<Bullet>();
        bt.damage = gunData.damage;
        bt.speed = gunData.bulletSpeed;
        bt.range = gunData.range;
        bt.AddSpeed(AimAtTarget());
        Instantiate(muzzleFlash, muzzle.position, mainCamera.transform.rotation, muzzle);
    }

    protected override void StartReloadAnimation(){
        animator.SetTrigger("Reload");
    }

    protected override void StartShotAnimation(){
        int randomNumber = UnityEngine.Random.Range(0, 4);
        switch (randomNumber) {
            case 0:
                animator.SetTrigger("Shoot1");
                break;
            case 1:
                animator.SetTrigger("Shoot2");
                break;
            case 2:
                animator.SetTrigger("Shoot3");
                break;
            case 3:
                animator.SetTrigger("Shoot4");
                break;
        
        }
    }
}
