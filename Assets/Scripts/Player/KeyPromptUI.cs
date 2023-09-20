using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyPromptUI : MonoBehaviour
{
    [SerializeField] GameObject keyHolder;
    [SerializeField] TextMeshProUGUI keyText;

    private bool isArmed = false;
    private string currentAction = "";

    Controls controls;

    void ShowPrompt(string actionName)
    {
        print("Showing PROMPT!");
        isArmed = true;
        keyHolder.SetActive(true);
        currentAction = actionName;
        keyText.text = controls.keyList[currentAction].ToString();
    }

    void HidePrompt()
    {
        isArmed = false;
        keyHolder.SetActive(false);
        currentAction = "";
    }

    private void Start()
    {
        controls = GameSettings.gameSettings.controls;
        SendKeyPrompt.OnKeyPrompt += ShowPrompt;
    }

    private void OnDisable()
    {
        SendKeyPrompt.OnKeyPrompt -= ShowPrompt;
    }

    private void Update()
    {
        if (currentAction != "" && Input.GetKeyDown(controls.keyList[currentAction]) && isArmed)
        {
            HidePrompt();
        }
    }
}
