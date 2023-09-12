using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionAndWeaponManager : MonoBehaviourWithPause{

    [SerializeField] Camera mainCamera;
    [SerializeField] float range;
    [SerializeField] List<Gun> gunList;//0 axe, 1 pistol,2 shotgun
    [SerializeField] PlayerUI UI;

    PlayerInput input;
    ShopManager shop;

    enum Weapons{
        Axe,
        Pistol,
        Shotgun,
        None
    }

    Weapons activeWeapon=Weapons.None;

    void Start(){
        shop=GetComponent<ShopManager>();
        input=GetComponent<PlayerInput>();
        ChangeActiveWeapon(Weapons.Pistol);
    }

     void DisplayAmmo(int magAmmo, int ammoInReserve){
        UI.DisplayAmmo(magAmmo, ammoInReserve);
    }

    public void StartCrosshairReload(float duration){
        UI.StartCrosshairReload(duration);
    }

    public void ChangeCrosshairScale(float scale){
        UI.ChangeCrosshairScale(scale);
    }

    public void ChangeCrosshairActivationState(bool state){
        UI.ChangeCrosshairActivationState(state);
    }

    void ChangeActiveWeapon(Weapons pWeapon) {



        if (activeWeapon != Weapons.None) {

            if (gunList[(int)activeWeapon].state != Gun.States.Idle)
                return;

            if (gunList[(int)activeWeapon].gameObject != null)
                gunList[(int)activeWeapon].gameObject.SetActive(false);

            gunList[(int)activeWeapon].OnAmmoChange -= DisplayAmmo;
            gunList[(int)activeWeapon].OnSpreadChange -= ChangeCrosshairScale;
            gunList[(int)activeWeapon].OnReload -= StartCrosshairReload;
            gunList[(int)activeWeapon].OnZoomChange -= ChangeCrosshairActivationState;
        }

        activeWeapon = pWeapon;

        if (activeWeapon != Weapons.None) {

            if (gunList[(int)activeWeapon].gameObject != null)
                gunList[(int)activeWeapon].gameObject.SetActive(true);

            DisplayAmmo(gunList[(int)activeWeapon].currentAmmo, gunList[(int)activeWeapon].extraAmmo);

            gunList[(int)activeWeapon].OnAmmoChange += DisplayAmmo;
            gunList[(int)activeWeapon].OnSpreadChange += ChangeCrosshairScale;
            gunList[(int)activeWeapon].OnReload += StartCrosshairReload;
            gunList[(int)activeWeapon].OnZoomChange += ChangeCrosshairActivationState;
        }
    }

    private void OnDestroy(){
        gunList[(int)activeWeapon].OnAmmoChange -= DisplayAmmo;
        gunList[(int)activeWeapon].OnSpreadChange -= ChangeCrosshairScale;
        gunList[(int)activeWeapon].OnReload -= StartCrosshairReload;
        gunList[(int)activeWeapon].OnZoomChange -= ChangeCrosshairActivationState;
    }


    protected override void UpdateWithPause(){

        foreach (Weapons weapon in Enum.GetValues(typeof(Weapons))) {

            if ((int)weapon >= gunList.Count)
                continue;

            if (gunList[(int)weapon] == null)
                continue;

            if (!gunList[(int)weapon].canBeAccessed)
                continue;

            int number = 49 + (int)weapon;

            if (Input.GetKeyDown((KeyCode)number)){
                ChangeActiveWeapon((Weapons)(number-49));
            }
        }

        CheckForInteractions();

    }

    void CheckForInteractions() {

        RaycastHit hitInfo;
        Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hitInfo, range);
        if (hitInfo.collider == null)
            return;
        if (input.interactionInput){

            switch (hitInfo.collider.gameObject.tag) {
                case "Interactable":
                    PickUpDrops(hitInfo);
                    break;
                case "Log":
                    PickUpLog(hitInfo);
                    break;
            }
        }
    }

    void PickUpDrops(RaycastHit pHitInfo) {
        string item = pHitInfo.collider.GetComponent<DropPod>().item;
        int amount = pHitInfo.collider.GetComponent<DropPod>().amount;
        switch (item){
            case "PistolAmmo":
                gunList[(int)Weapons.Pistol].extraAmmo += amount;
                if (activeWeapon == Weapons.Pistol){
                    DisplayAmmo(gunList[(int)activeWeapon].currentAmmo, gunList[(int)activeWeapon].extraAmmo);
                }
                break;
            case "Shotgun":
                gunList[(int)Weapons.Shotgun].canBeAccessed = true;
                break;
        }
        Destroy(pHitInfo.collider.gameObject);
    }

    void PickUpLog(RaycastHit pHitInfo) {
        shop.AddMoney(pHitInfo.collider.gameObject.GetComponent<Tree>()._value);
        Destroy(pHitInfo.collider.gameObject);
    }


    public bool CheckActiveGunisAiming() {
        return gunList[(int)activeWeapon].isAiming;
    }
}
