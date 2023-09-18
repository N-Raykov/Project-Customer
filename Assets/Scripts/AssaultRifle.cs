using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Gun{

    private void Awake(){
        canBeAccessed = true;
        gameObject.SetActive(false);
    }

    protected override void StartReloadAnimation(){
        
    }

    protected override void StartShotAnimation()
    {
        
    }
}
