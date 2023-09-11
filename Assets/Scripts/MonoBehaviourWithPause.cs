using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourWithPause : MonoBehaviour{

    protected bool ignorePausedState=false;

    void Update(){
        if (!GameManager.gameIsPaused||ignorePausedState)
            UpdateWithPause();
    }

    void FixedUpdate(){
        if (!GameManager.gameIsPaused||ignorePausedState) 
            FixedUpdateWithPause();
    }

    protected virtual void FixedUpdateWithPause() { }
    protected virtual void UpdateWithPause() { }
}
