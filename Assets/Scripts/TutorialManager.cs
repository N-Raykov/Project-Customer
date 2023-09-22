using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviourWithPause {

    [SerializeField] GameObject spawner;
    [SerializeField] TextMeshProUGUI objective;
    [SerializeField] PlayerInput input;
    [SerializeField] Revolver revolver;
    ShopManager shopManager;
    Controls controls;

    int phase = 1;
    

    private void Start(){
        ignorePausedState = true;
        //spawner.SetActive(false);
        spawner.GetComponent<EnemySpawner>().enabled = false;
        controls = Controls.controls;
        objective.text = "You are wounded! Use your axe to chop one of the highlighted trees to recover your health.";
        shopManager = input.GetComponent<ShopManager>();
    }

    protected override void UpdateWithPause(){
        EnableSpawner();
        switch (phase) {
            case 1:
                if (GameManager.fallenTrees > 0)
                    phase = 2;
                break;
            case 2:
                objective.text = string.Format("You can sell the fallen log for money by pressing {0} while looking at it",controls.keyList["interact"]);
                if (shopManager.money > 0) {
                    phase = 3;
                }
                break;
            case 3:
                objective.text = string.Format("Switch to your revolver by pressing {0}", controls.keyList["revolver"]);
                if (Input.GetKeyDown(controls.keyList["revolver"])) {
                    phase = 4;
                }
                break;
            case 4:
                objective.text = string.Format("Oh no! You are out of ammo! Open the shop by pressing {0} and buy some ammo",controls.keyList["shop"]);
                if (shopManager.shopIsActive) {
                    phase = 5;
                }
                break;
            case 5:
                objective.text = string.Format("Your purchases drop down from the sky somewhere close to you! You can pick them up by pressing {0}", controls.keyList["interact"]);
                if (revolver.GetExtraAmmo()>0) {
                    Debug.Log("IWANTDOFUCKINGDIEEEIEIAIEIEAIEIAWEJKIA");
                    phase = 6;
                }
                break;
            case 6:
                objective.text = string.Format("Your goal is to cut down all big trees! For this you can down you helper robot by pressing {0}", controls.keyList["robotSpawn"]);
                if (input.spawnBot){
                    phase = 7;
                    spawner.GetComponent<EnemySpawner>().enabled = true;
                }
                break;
            case 7:
                objective.text = "Waves of enemies will spawn when he will start cutting down the big tree. Protect him until he finishes";
                if (spawner.GetComponent<EnemySpawner>().treesCut != 0) {
                    phase = 8;
                }
                break;
            case 8:
                objective.text = "From now on you are on your own! Cut down all the big trees and return home!";
                break;
        
        }
        
    }

    void EnableSpawner() {
        if (input.spawnBot) {
            spawner.GetComponent<EnemySpawner>().enabled = true;
        }

    }
}
