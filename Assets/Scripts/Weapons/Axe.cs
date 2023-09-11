using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Gun {

    private void Awake(){
        canBeAccessed = true;
        gameObject.SetActive(false);
    }

    protected override void CheckForActions() {
        CheckForShots();
    }

    protected override void Shoot() {
        StartShotAnimation();
        lastShotTime = Time.time;
        state = States.Shoot;
        StartCoroutine(ChangeStateAfterTime(gunData.shotCooldown, States.Idle));
    }

    protected override void StartShotAnimation(){
        animator.SetTrigger("Shoot");
    }

    protected override void StartReloadAnimation(){
        
    }
}
