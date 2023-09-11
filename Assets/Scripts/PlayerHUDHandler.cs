using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHUDHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject[] WeaponSelectors;


    //TODO: Replace with correct events later
    private void OnEnable()
    {
        HUDInputTest.onWeaponChange += SelectWeaponUI;
    }

    private void OnDisable()
    {
        HUDInputTest.onWeaponChange -= SelectWeaponUI;
    }

    void TweenWeaponSelect(int weaponIndex, float endMargin)
    {
        RectTransform selectedWeaponTransform = WeaponSelectors[weaponIndex].GetComponent<RectTransform>();
        selectedWeaponTransform.DOSizeDelta(new Vector2(endMargin, 85), 0.5f);
    }

    void SelectWeaponUI(int weaponIndex)
    {
        for (int i = 0; i < WeaponSelectors.Length; i++)
        {
            if (i == weaponIndex) { continue; }
            TweenWeaponSelect(i, -150);
        }

        TweenWeaponSelect(weaponIndex, 0);
    }
}
