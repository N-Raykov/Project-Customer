using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class GameSettings : MonoBehaviour {

    [SerializeField] TextMeshProUGUI grapple;
    [SerializeField] AudioMixer audioMixer;

    public static float volume { get; private set; }
    public static KeyCode grappleKey { get; private set; }
    public static KeyCode shootKey { get; private set; }
    public static KeyCode reloadKey { get; private set; }

    bool canCheckForInputGrapple = false;

    private void Awake(){
        grappleKey = KeyCode.E;
        shootKey = KeyCode.Mouse0;
        reloadKey = KeyCode.R;
    }
    private void Start(){
        if (grapple!=null)
            grapple.text = "" + grappleKey;
    }
    public void SetVolume(float pVolume) {
        audioMixer.SetFloat("MasterVolume",Mathf.Log10(pVolume)*20);
        volume = pVolume;
        //Debug.Log(volume);
    }

    public void SetGrappleKey(KeyCode key) {
        grappleKey = key;
    }
    public void SetShootKey(KeyCode key){
        shootKey = key;
    }
    public void SetReloadKey(KeyCode key){
        reloadKey = key;
    }

    private void Update(){
        if (canCheckForInputGrapple) {
            for (int i = 0; i < 400; i++){
                if (Input.GetKeyDown((KeyCode)i)){
                    Debug.Log("Key pressed: " + i);
                    grappleKey = (KeyCode)i;
                    grapple.text = "" + grappleKey;
                    canCheckForInputGrapple = false;
                    break;
                }
            }

        }
    }

    public void EnableGrappleCheck() {
        canCheckForInputGrapple = true;
    }
}
