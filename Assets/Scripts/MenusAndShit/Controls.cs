using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviourWithPause {
    public Dictionary<string, KeyCode> keyList { get; set; }
    void Awake() {
        keyList = new Dictionary<string, KeyCode>();
        //movement
        keyList.Add("jump",KeyCode.Space);
        //interaction
        keyList.Add("interact", KeyCode.E);
        keyList.Add("shop", KeyCode.H);
        //combat
        keyList.Add("shoot", KeyCode.Mouse0);
        keyList.Add("aim", KeyCode.Mouse1);
        keyList.Add("reload", KeyCode.R);
        keyList.Add("ability1", KeyCode.Q);

    }

}
