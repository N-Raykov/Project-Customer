using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerHUDHandler : MonoBehaviour
{   
    [SerializeField] private GameObject[] weaponSelectors;
    //[SerializeField] private GameObject[] abillityIndicators;

    [SerializeField] private TextMeshProUGUI objectiveLabel;
    [SerializeField] private TextMeshProUGUI moneyLabel;

    private int currentWeaponIndex = 0;

    //TODO: Replace with correct events later
    private void OnEnable()
    {
        HUDInputTest.onWeaponChange += SelectWeaponUI;
        HUDInputTest.onObjectiveUpdated += UpdateObjective;
        HUDInputTest.onMoneyChange += UpdateMoneyUI;
        Gun.OnAmmoChange += UpdateAmmoUI;
    }

    private void OnDisable()
    {
        HUDInputTest.onWeaponChange -= SelectWeaponUI;
        HUDInputTest.onObjectiveUpdated -= UpdateObjective;
        HUDInputTest.onMoneyChange -= UpdateMoneyUI;
        Gun.OnAmmoChange -= UpdateAmmoUI;
    }

    void TweenWeaponSelect(int weaponIndex, float endMargin)
    {
        RectTransform selectedWeaponTransform = weaponSelectors[weaponIndex].GetComponent<RectTransform>();
        selectedWeaponTransform.DOSizeDelta(new Vector2(endMargin, 85), 0.5f);
    }

    void SelectWeaponUI(int weaponIndex)
    {
        currentWeaponIndex = weaponIndex;

        for (int i = 0; i < weaponSelectors.Length; i++)
        {
            if (i == weaponIndex) { continue; }
            TweenWeaponSelect(i, -150);
            GameObject deselectedWeaponUI = weaponSelectors[i];
            deselectedWeaponUI.GetComponent<WeaponUIContainer>().SetAmmoLabelVisibility(false);
        }

        TweenWeaponSelect(weaponIndex, 0);
        GameObject selectedWeaponUI = weaponSelectors[weaponIndex];
        selectedWeaponUI.GetComponent<WeaponUIContainer>().SetAmmoLabelVisibility(true);
    }

    void UpdateAmmoUI(int currentAmmo, int ammoLeft)
    {
        GameObject selectedWeaponUIObject = weaponSelectors[currentWeaponIndex];
        selectedWeaponUIObject.GetComponent<WeaponUIContainer>().UpdateAmmo(currentAmmo, ammoLeft);
    }

    void UpdateObjective(string newObjective)
    {
        objectiveLabel.text = "• " + newObjective;
    }

    void UpdateMoneyUI(int currentMoney)
    {
        moneyLabel.text = "$" + currentMoney.ToString();
    }

}
