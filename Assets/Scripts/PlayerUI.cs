using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject crosshair;
    [SerializeField] RectTransform crosshairSidesHolder;
    [SerializeField] RectTransform reloadCrosshair;
    Image reloadImage;

    Vector3 crosshairScaleStart;
    
    void Awake(){
        //text = GetComponent<TextMeshProUGUI>();
        crosshairScaleStart = crosshairSidesHolder.localScale;
        Gun.OnAmmoChange += DisplayAmmo;
        Gun.OnSpreadChange += ChangeCrosshairScale;
        Gun.OnReload += StartCrosshairReload;
        Gun.OnZoomChange += ChangeCrosshairActivationState;
        reloadImage = reloadCrosshair.GetComponent<Image>();
        reloadImage.fillAmount = 0;
        reloadImage.enabled = false;
    }

    void StartCrosshairReload(float duration) {
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

    void ChangeCrosshairScale(float scale) {
        crosshairSidesHolder.localScale = crosshairScaleStart * scale;
    }

    void DisplayAmmo(int magAmmo,int ammoInReserve) {
        text.text = String.Format("Ammo : {0} / {1}" , magAmmo , ammoInReserve);
    }

    private void OnDestroy(){
        Gun.OnAmmoChange -= DisplayAmmo;
        Gun.OnSpreadChange -= ChangeCrosshairScale;
        Gun.OnReload -= StartCrosshairReload;
    }
    void ChangeCrosshairActivationState(bool state) {
        crosshair.SetActive(state);
    }
}
