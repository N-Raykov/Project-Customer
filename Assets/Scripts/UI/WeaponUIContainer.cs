using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUIContainer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI ammoLabel;

    public void UpdateAmmo(int currentAmmo, int ammoLeft)
    {
        ammoLabel.text = currentAmmo.ToString() + " / " + ammoLeft.ToString();
    }

    public void SetAmmoLabelVisibility(bool status)
    {
        ammoLabel.enabled = status;
    }
}
