using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviourWithPause {

    public static GameSettings gameSettings { get; private set; }

    public Controls controls { get; private set; }

    [SerializeField] TextMeshProUGUI jump;
    [SerializeField] TextMeshProUGUI interact;
    [SerializeField] TextMeshProUGUI shop;
    [SerializeField] TextMeshProUGUI shoot;
    [SerializeField] TextMeshProUGUI aim;
    [SerializeField] TextMeshProUGUI reload;
    [SerializeField] TextMeshProUGUI ability;

    Dictionary<string,TextMeshProUGUI> textLinks=new Dictionary<string, TextMeshProUGUI>();

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] GameObject UI;
    Button lastPressedButton;

    public static float volume { get; private set; }


    bool canCheckForInput = false;
    string selectedKey;

    private void Awake(){
        ignorePausedState = true;

        if (gameSettings != null)
            Destroy(gameObject);
        else{
            DontDestroyOnLoad(gameObject);
            gameSettings = this;
        }


        

    }
    private void Start(){
        controls=GetComponent<Controls>();

        textLinks.Add("jump",jump);
        textLinks.Add("interact", interact);
        textLinks.Add("shop", shop);
        textLinks.Add("shoot", shoot);
        textLinks.Add("aim", aim);
        textLinks.Add("reload", reload);
        textLinks.Add("ability", ability);


    }
    public void SetVolume(float pVolume) {
        audioMixer.SetFloat("MasterVolume",Mathf.Log10(pVolume)*20);
        volume = pVolume;
    }


    protected override void UpdateWithPause(){
        if (SceneManager.GetActiveScene().name != "MainMenu")
            UI.SetActive(false);

        if (canCheckForInput) {
            for (int i = 0; i < 400; i++){
                if (Input.GetKeyDown((KeyCode)i)){

                    ColorBlock cb = lastPressedButton.colors;
                    Color color1 = cb.normalColor;
                    color1.a = 0f;
                    cb.normalColor = color1;
                    Color color2 = cb.highlightedColor;
                    color2.a = 0.1490196f;
                    cb.highlightedColor = color2;
                    lastPressedButton.colors = cb;

                    canCheckForInput = false;

                    if ((KeyCode)i == KeyCode.Escape)
                        break;

                    controls.keyList[selectedKey] = (KeyCode)i;
                    textLinks[selectedKey].text = "" + controls.keyList[selectedKey];

                    Debug.Log("Key pressed: " + i);
                    break;
                }
            }

        }
    }

    public void FindInputAndEnableCheck(string pInputName) {
        selectedKey = pInputName;
        canCheckForInput = true;
        
    }

    public void OnClicked(Button button){
        lastPressedButton = button;
        ColorBlock cb = button.colors;
        Color color1 = cb.normalColor;
        color1.a = 0.5f;
        cb.normalColor = color1;
        Color color2 = cb.highlightedColor;
        color2.a = 0.5f;
        cb.highlightedColor = color2;
        button.colors = cb;

    }

}
