using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun{


    private void Awake(){
        //canBeAccessed = false;
    }

    protected override void StartReloadAnimation(){
        Debug.Log("reload");
        //throw new System.NotImplementedException();
    }
    protected override void StartShotAnimation()
    {
        Debug.Log("shoot");
        //throw new System.NotImplementedException();
    }

}
