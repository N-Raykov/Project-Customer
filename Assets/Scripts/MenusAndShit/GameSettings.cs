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
    [SerializeField] TextMeshProUGUI robot;
    [SerializeField] TextMeshProUGUI shoot;
    [SerializeField] TextMeshProUGUI aim;
    [SerializeField] TextMeshProUGUI reload;
    [SerializeField] TextMeshProUGUI ability;
    [SerializeField] TextMeshProUGUI axe;
    [SerializeField] TextMeshProUGUI revolver;
    [SerializeField] TextMeshProUGUI shotgun;
    [SerializeField] TextMeshProUGUI assaultRifle;

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

        controls = GetComponent<Controls>();

    }
    private void Start(){

        textLinks.Add("jump",jump);
        textLinks.Add("interact", interact);
        textLinks.Add("shop", shop);
        textLinks.Add("robotSpawn", robot);
        textLinks.Add("shoot", shoot);
        textLinks.Add("aim", aim);
        textLinks.Add("reload", reload);
        textLinks.Add("ability1", ability);
        textLinks.Add("axe", axe);
        textLinks.Add("revolver", revolver);
        textLinks.Add("shotgun", shotgun);
        textLinks.Add("rifle", assaultRifle);

        textLinks["jump"].text = "" + controls.keyList["jump"];
        textLinks["interact"].text = "" + controls.keyList["interact"];
        textLinks["shop"].text = "" + controls.keyList["shop"];
        textLinks["robotSpawn"].text = "" + controls.keyList["robotSpawn"];
        textLinks["shoot"].text = "" + controls.keyList["shoot"];
        textLinks["aim"].text = "" + controls.keyList["aim"];
        textLinks["reload"].text = "" + controls.keyList["reload"];
        textLinks["ability1"].text = "" + controls.keyList["ability1"];
        textLinks["axe"].text = "" + controls.keyList["axe"];
        textLinks["revolver"].text = "" + controls.keyList["revolver"];
        textLinks["shotgun"].text = "" + controls.keyList["shotgun"];
        textLinks["rifle"].text = "" + controls.keyList["rifle"];

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
