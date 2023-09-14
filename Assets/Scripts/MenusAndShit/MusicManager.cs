using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour{

    public static MusicManager musicManager { get; private set; }

    private void Awake(){
        if (musicManager != null)
            Destroy(gameObject);
        else {
            DontDestroyOnLoad(gameObject);
            musicManager = this;
        }
    }
}
