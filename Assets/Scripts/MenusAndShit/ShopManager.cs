using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
public class ShopManager : MonoBehaviourWithPause{

    [Header("UI")]
    [SerializeField] GameObject ammoUI;
    [SerializeField] GameObject crosshairUI;
    [SerializeField] GameObject shopUI;
    [SerializeField] TextMeshProUGUI purchasePanel;
    [SerializeField] PlayerUI UI;
    [SerializeField] Color orange;

    [Header("Info")]
    [SerializeField] GameObject dropPod;
    PlayerInput input;

    [Header("DropInfo")]
    [SerializeField] float dropPodRangeMax;
    [SerializeField] float dropPodRangeMin;
    [SerializeField] float dropPodSize;
    [SerializeField] LayerMask mask;
    [SerializeField] LayerMask ground;

    [Header("Purchases")]
    [SerializeField] int maxPurchases;
    int purchases = 0;

    [SerializeField] GameObject weaponsPage;
    [SerializeField] GameObject suppliesPage;
    [SerializeField] GameObject abilitiesPage;

    public int money { get; private set; }
    public bool shopIsActive { get; private set; }
    Rigidbody rb;

    Image lastImageChanged = null;
    GameObject activePage;
    Button lastDisabledButton;
    CollisionCheckForDropboxes[] spawnPointsTemp;
    List<Vector3> spawnPoints;//when spawning use player.transform.position+spawnpoint[1];
    List<Vector3> placesToSpawn = new List<Vector3>();//the ones where drop pods will actually spawn
        
    void Start(){
        shopIsActive = false;
        spawnPoints = new List<Vector3>();
        spawnPointsTemp = GameObject.FindObjectsOfType<CollisionCheckForDropboxes>();
        //foreach (CollisionCheckForDropboxes c in spawnPointsTemp) {
        //    spawnPoints.Add(c.transform.localPosition);
        //}

        activePage = null;
        lastDisabledButton = null;
        money = 0;
        UI.DisplayCash(money);
        rb = GetComponent<Rigidbody>();
        ignorePausedState = true;
        input = GetComponent<PlayerInput>();
        //ChangeInterfaceState(false);
    }

    protected override void UpdateWithPause(){

        //Debug.Log(spawnPoints.Count);

        if (input.shopInput) {
            ChangeInterfaceState(!shopIsActive);
        }
    }

    public void ChangeInterfaceState(bool pState) {
        purchases = 0;
        UpdatePurchasePanel();
        Cursor.visible = pState;
        Cursor.lockState = (CursorLockMode)((pState?1:0)+1);
        GameManager.gameIsPaused = pState;
        Time.timeScale = Math.Abs(Time.timeScale - 1);
        shopIsActive = pState;
        ammoUI.SetActive(!pState);
        crosshairUI.SetActive(!pState);
        shopUI.SetActive(pState);
        if (activePage != null)
        {
            activePage.SetActive(false);
            activePage = null;
        }
        if (lastDisabledButton != null) {
            lastDisabledButton.enabled = true;
            lastDisabledButton = null;
        }

        if (lastImageChanged != null)
        {
            Debug.Log(1);
            lastImageChanged.color = new Color(255, 255, 255);
            lastImageChanged = null;
        }

        spawnPoints = new List<Vector3>();
        foreach (CollisionCheckForDropboxes c in spawnPointsTemp){
            if (c.canBeSpawnedOn)
                spawnPoints.Add(c.transform.localPosition);
        }
        Debug.Log(spawnPoints.Count);

        if (placesToSpawn.Count!=0){
            foreach (Vector3 v in placesToSpawn)
                spawnPoints.Add(v);
            placesToSpawn = new List<Vector3>();
        }

    }
    public void SpawnDropPod(ShopButtonData pData){

        int spawnPointListLocation = UnityEngine.Random.Range(0, spawnPoints.Count);
        Vector3 spawnPoint = transform.position + spawnPoints[spawnPointListLocation]+new Vector3(0,110,0);
        placesToSpawn.Add(spawnPoint);
        spawnPoints.RemoveAt(spawnPointListLocation);
        Debug.Log(spawnPoints.Count);

        DropPod dp = (Instantiate(dropPod, spawnPoint, Quaternion.identity)).GetComponent<DropPod>();
        dp.data = pData;
        RaycastHit groundCheck;
        Physics.Raycast(spawnPoint, Vector3.down, out groundCheck, 10000, ground);
        dp.distanceToGround = groundCheck.distance;
        purchases++;
        UpdatePurchasePanel();

    }

    void UpdatePurchasePanel() {
        purchasePanel.text = String.Format("{0} / {1} max purchases", purchases, maxPurchases);
    }

    public void BuyItem(ShopButtonData pData) {
        if (purchases >= maxPurchases)
            return;

        if (pData.cost > money||(pData.stock==0))
            return;

        money -= pData.cost;
        UI.DisplayCash(money);
        pData.stock--;
        SpawnDropPod(pData);
    }

    public void ActivatePage(GameObject pGameObject) {

        if (activePage != null)
            activePage.SetActive(false);

        pGameObject.SetActive(true);
        activePage = pGameObject;
    }

    public void DisableButton(Button pButton) {
        if (lastDisabledButton != null)
            lastDisabledButton.enabled = true;

        if (lastImageChanged != null){
            lastImageChanged.color = new Color(255, 255, 255);
            lastImageChanged = null;
        }

        if (pButton != null){
            lastDisabledButton = pButton;
            pButton.enabled = false;
        }
    }

    public void BackToPreviousPage(ShopButtonData pData) {
        if (activePage != null)
            activePage.SetActive(false);
        switch (pData.type) {
            case "weapons":
                weaponsPage.SetActive(true);
                activePage = weaponsPage;
                break;
            case "supplies":
                suppliesPage.SetActive(true);
                activePage = suppliesPage;
                break;
            case "abilities":
                abilitiesPage.SetActive(true);
                activePage = abilitiesPage;
                break;
            default:
                Debug.Log("fix the game");
                break;
        }
        
    }

    public void AddMoney(int pMoney) {
        money += pMoney;
        UI.DisplayCash(money);
    }

    public void RemoveFromSpawnpoints(Vector3 vector3) {

        for (int i = 0; i < spawnPoints.Count;i++) {
            if (spawnPoints[i] == vector3) {
                Debug.Log("working");
                spawnPoints.RemoveAt(i);
                i--;
            }
        }
        //Debug.Log(spawnPoints.Remove(vector3));
        //Debug.Log(vector3 + " " + spawnPoints.Count);
    }
    public void AddtoSpawnpoints(Vector3 vector3){

        spawnPoints.Add(vector3);
        Debug.Log(1);
        //Debug.Log(vector3 + " " + spawnPoints.Count);

    }

    public void ChangeImageColor(Image pImage) {
        pImage.color = orange;
        lastImageChanged = pImage;
    }
}
