using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviourWithPause{

    [SerializeField] GameObject panel;
    bool pauseState = true;
    private void Start(){
        ignorePausedState = true;
    }

    protected override void UpdateWithPause(){
        if (Input.GetKeyDown(GameSettings.gameSettings.controls.keyList["pause"])) {
            Debug.Log("working");
            ChangePauseState(!pauseState);
        }
    }

    public void ChangePauseState(bool pState) {
        pauseState = pState;
        panel.SetActive(pState);
    }
}
