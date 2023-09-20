using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;    

public class InteractionAndWeaponManager : MonoBehaviourWithPause{

    [Header("Data")]
    [SerializeField] Camera mainCamera;
    [SerializeField] float range;
    [SerializeField] List<Gun> gunList;//0 axe, 1 pistol,2 shotgun
    [SerializeField] PlayerUI UI;
    [SerializeField] GameObject ammoPanel;

    [Header("TreeHp")]
    [SerializeField] GameObject treeHpHolder;
    [SerializeField] TextMeshProUGUI treeHPText;
    [SerializeField] RectTransform treeHPTransform;

    [Header("SellMessage")]
    [SerializeField] GameObject treeSellMessageBackground;
    [SerializeField] TextMeshProUGUI treeSellMessage;

    PlayerInput input;
    ShopManager shop;
    DropPod lastDropPodSeen;

    enum Weapons{
        Axe,
        Pistol,
        Shotgun,
        AssaultRifle,
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

            if (gunList[(int)activeWeapon].isAiming)
                return;

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

        if (activeWeapon == Weapons.Axe)
            ammoPanel.SetActive(false);
        else
            ammoPanel.SetActive(true);

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
        treeHpHolder.SetActive(false);
        treeSellMessageBackground.SetActive(false);
        if (lastDropPodSeen != null) {
            lastDropPodSeen.ChangeUIState(false);
        }

        RaycastHit hitInfo;
        Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hitInfo, range);
        if (hitInfo.collider == null)
            return;

        Debug.Log(hitInfo.collider.gameObject.name);

        switch (hitInfo.collider.gameObject.tag){
            case "Interactable":
                lastDropPodSeen = hitInfo.collider.gameObject.gameObject.GetComponent<DropPod>();
                lastDropPodSeen.LookAtCamera();
                lastDropPodSeen.ChangeUIState(true);
                if (input.interactionInput)
                    PickUpDrops(hitInfo);
                break;
            case "Log":
                ShowTreeInfo(hitInfo);
                if (input.interactionInput)
                    PickUpLog(hitInfo);
                break;
            case "BigTree":
                BigTreeSellInfo(hitInfo);
                if (input.interactionInput)
                    PickUpLog(hitInfo);
                break;

        }
    }

    void BigTreeSellInfo(RaycastHit pHitInfo) {
        Tree tree = pHitInfo.collider.gameObject.GetComponent<Tree>();
        if (tree.hasFallen){
            treeSellMessageBackground.SetActive(true);
            treeSellMessage.text = String.Format("Sell for {0}$", tree._value);
            return;
        }
    }

    void PickUpDrops(RaycastHit pHitInfo) {
        string item = lastDropPodSeen.data.item;
        int amount = lastDropPodSeen.data.amount;
        switch (item){
            case "PistolAmmo":
                gunList[(int)Weapons.Pistol].extraAmmo += amount;
                if (activeWeapon == Weapons.Pistol){
                    DisplayAmmo(gunList[(int)activeWeapon].currentAmmo, gunList[(int)activeWeapon].extraAmmo);
                }
                break;
            case "ShotgunAmmo":
                gunList[(int)Weapons.Shotgun].extraAmmo += amount;
                if (activeWeapon == Weapons.Shotgun){
                    DisplayAmmo(gunList[(int)activeWeapon].currentAmmo, gunList[(int)activeWeapon].extraAmmo);
                }
                break;
            case "AssaultRifleAmmo":
                gunList[(int)Weapons.AssaultRifle].extraAmmo += amount;
                if (activeWeapon == Weapons.AssaultRifle)
                {
                    DisplayAmmo(gunList[(int)activeWeapon].currentAmmo, gunList[(int)activeWeapon].extraAmmo);
                }
                break;
            case "Shotgun":
                gunList[(int)Weapons.Shotgun].canBeAccessed = true;
                break;
            case "AssaultRifle":
                gunList[(int)Weapons.AssaultRifle].canBeAccessed = true;
                break;
            case "GravityWave":
                GetComponent<PlayerAbility>().MakeAbilityAvailable();
                break;
        }
        Destroy(pHitInfo.collider.gameObject);
    }

    void PickUpLog(RaycastHit pHitInfo) {
        Tree tree= pHitInfo.collider.gameObject.GetComponent<Tree>();
        if (tree.hasFallen) {
            shop.AddMoney(tree._value);
            Destroy(pHitInfo.collider.gameObject);
        }
    }

    void ShowTreeInfo(RaycastHit pHitInfo) {
        Tree tree = pHitInfo.collider.gameObject.GetComponent<Tree>();
        if (tree._hp > 0) {
            treeHpHolder.SetActive(true);
            treeHPText.text = String.Format("{0}/{1} HP",tree._hp,tree._maxHP);
            treeHPTransform.localScale = new Vector3(((float)tree._hp) / tree._maxHP, 1, 1);
            return;
        }
        if (tree.hasFallen) {
            treeSellMessageBackground.SetActive(true);
            treeSellMessage.text = String.Format("Sell for {0}$",tree._value);
            return;
        }
    }

    public bool CheckActiveGunisAiming() {
        return gunList[(int)activeWeapon].isAiming;
    }
}
