using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviourWithPause {
    public static Controls controls { get; private set; }

    public Dictionary<string, KeyCode> keyList { get; set; }

    void Awake() {
        if (controls != null) {
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
            controls = this;

            keyList = new Dictionary<string, KeyCode>();
            //movement
            keyList.Add("jump", KeyCode.Space);
            keyList.Add("sprint", KeyCode.LeftShift);
            //interaction
            keyList.Add("interact", KeyCode.E);
            keyList.Add("shop", KeyCode.H);
            keyList.Add("robotSpawn", KeyCode.G);
            keyList.Add("pause", KeyCode.Tab);
            //combat
            keyList.Add("shoot", KeyCode.Mouse0);
            keyList.Add("aim", KeyCode.Mouse1);
            keyList.Add("reload", KeyCode.R);
            keyList.Add("ability1", KeyCode.Q);
            keyList.Add("axe", KeyCode.Alpha1);
            keyList.Add("revolver", KeyCode.Alpha2);
            keyList.Add("shotgun", KeyCode.Alpha4);
            keyList.Add("rifle", KeyCode.Alpha3);
        }

    }

}
