using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviourWithPause{
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] GameObject crosshair;
    [SerializeField] RectTransform crosshairSidesHolder;
    [SerializeField] RectTransform reloadCrosshair;
    Image reloadImage;

    Vector3 crosshairScaleStart;
    
    void Awake(){//it would be better if it was start
        crosshairScaleStart = crosshairSidesHolder.localScale;
        reloadImage = reloadCrosshair.GetComponent<Image>();
        reloadImage.fillAmount = 0;
        reloadImage.enabled = false;
    }

    public void StartCrosshairReload(float duration) {
        StartCoroutine(CrosshairReload(duration));
    }

    IEnumerator CrosshairReload(float duration) {
        reloadImage.enabled = true;
        for (int i = 0; i < 100; i++) {
            reloadImage.fillAmount += 0.01f;
            yield return new WaitForSeconds(duration/100);
        }
        yield return new WaitForEndOfFrame();
        reloadImage.fillAmount = 0f;
        reloadImage.enabled = false;
    }

    public void ChangeCrosshairScale(float scale) {
        crosshairSidesHolder.localScale = crosshairScaleStart * scale;
    }

    public void DisplayAmmo(int magAmmo,int ammoInReserve) {
        ammoText.text = String.Format("Ammo : {0} / {1}" , magAmmo , ammoInReserve);
    }

    public void ChangeCrosshairActivationState(bool state) {
        crosshair.SetActive(state);
    }

    public void DisplayCash(int cash) {
        moneyText.text = String.Format("{0}$", cash);
    }
}
