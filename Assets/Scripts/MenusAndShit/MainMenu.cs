using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{
    [SerializeField]string scene;
    public void Play() {
        SceneManager.LoadScene(scene);
    }

    public void Quit() {
        Debug.Log("closed");
        Application.Quit();
    }
}
