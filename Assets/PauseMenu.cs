using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviourWithPause{

    [SerializeField] GameObject panel;
    [SerializeField] GameObject playerUI;
    bool pauseState = false;
    private void Start(){
        ignorePausedState = true;
    }

    protected override void UpdateWithPause(){
        if (Input.GetKeyDown(Controls.controls.keyList["pause"])) {
            Debug.Log("working");
            ChangePauseState(!pauseState);
            //panel.SetActive(true);
        }
    }

    public void ChangePauseState(bool pState) {

        pauseState = pState;
        panel.SetActive(pState);
        playerUI.SetActive(!pState);
        Cursor.visible = pState;
        Cursor.lockState = (CursorLockMode)((pState ? 1 : 0) + 1);
        GameManager.gameIsPaused = pState;
        Time.timeScale = Mathf.Abs(Time.timeScale - 1);
    }
}
