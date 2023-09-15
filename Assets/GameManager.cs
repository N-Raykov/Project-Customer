using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
    public static GameManager gameManager { get; private set; }

    public static bool gameIsPaused { get; set; }

    public static int fallenTrees { get; set; }

    private void Awake(){
        if (gameManager != null)
            Destroy(gameObject);
        else{
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
    }
}
