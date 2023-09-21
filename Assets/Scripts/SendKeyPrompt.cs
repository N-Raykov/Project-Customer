using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SendKeyPrompt : MonoBehaviour
{
    public static event Action<string> OnKeyPrompt;

    public void SendPrompt(string actionName)
    {
        OnKeyPrompt?.Invoke(actionName);
    }
}
