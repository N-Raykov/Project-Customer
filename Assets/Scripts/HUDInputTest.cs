using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HUDInputTest : MonoBehaviour
{
    public static event Action<int> onWeaponChange;
    private int tapCount = 0;

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
    }
}
