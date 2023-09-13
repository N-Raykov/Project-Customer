using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourWithPause{
    public static GameManager gameManager { get; private set; }

    public static bool gameIsPaused { get; set; }

    public static int fallenTrees { get; set; }

    public static GameObject robot{ get; set; }

    private void Awake(){
        if (gameManager != null)
            Destroy(gameObject);
        else{
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
    }
}
