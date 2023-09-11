using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HUDInputTest : MonoBehaviour
{
    //TODO: THIS SCRIPT IS ONLY MEANT FOR TESTING!
    //PUT THE CORRECT EVENTS IN THE APPROPRIATE SCRIPT LATER IN PRODUCTION!
    public static event Action<int> onWeaponChange;
    public static event Action<String> onObjectiveUpdated;
    public static event Action<int> onMoneyChange;
    public static event Action<int> onAbillityCast;

    private int tapCount = 0;

    //TESTING CURRENCY TO TEST UI ONLY
    private int currentMoney = 0;

    private void Start()
    {
        onWeaponChange?.Invoke(0);
    }

    private IEnumerator startAbillityCooldown(int AbillityNumber)
    {
        yield return new WaitForSeconds(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            onWeaponChange?.Invoke(tapCount);
            tapCount++;
            if (tapCount > 2)
            {
                tapCount = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            onObjectiveUpdated?.Invoke("Cut down the trees");
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            onMoneyChange?.Invoke(currentMoney++);
        } else if (Input.GetKeyDown(KeyCode.Period))
        {
            onMoneyChange?.Invoke(currentMoney--);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            onAbillityCast?.Invoke(0);
        } else if (Input.GetKeyDown(KeyCode.F)) {
            onAbillityCast?.Invoke(1);
        }
    }
}
