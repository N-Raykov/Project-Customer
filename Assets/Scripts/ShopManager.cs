using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class ShopManager : MonoBehaviourWithPause{

    [Header("UI")]
    [SerializeField] GameObject ammoUI;
    [SerializeField] GameObject crosshairUI;
    [SerializeField] GameObject shopUI;
    [SerializeField] TextMeshProUGUI purchasePanel;

    [Header("Info")]
    //[SerializeField] Dictionary<string,GameObject> shopPages;
    [SerializeField] GameObject dropPod;
    PlayerInput input;

    [Header("DropInfo")]
    [SerializeField] float dropPodRangeMax;
    [SerializeField] float dropPodRangeMin;
    [SerializeField] float dropPodSize;
    [SerializeField]LayerMask mask;

    [Header("Purchases")]
    [SerializeField] int maxPurchases;
    int purchases = 0;

    public int money { get; private set; }
    bool shopIsActive = false;
    Rigidbody rb;

    Action OnButtonClick;
    
    void Start(){
        money = 60;
        rb = GetComponent<Rigidbody>();
        ignorePausedState = true;
        input = GetComponent<PlayerInput>();
    }

    protected override void UpdateWithPause(){
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
    }

    public void SpawnDropPod(ShopButtonData pData) {

        Vector3 spawnPoint = new Vector3(0,110,0);

        int randomXOrientation = UnityEngine.Random.Range(0, 2);
        int randomZOrientation = UnityEngine.Random.Range(0, 2);

        spawnPoint.x = rb.position.x + UnityEngine.Random.Range(dropPodRangeMin, dropPodRangeMax) * ((randomXOrientation == 0) ? -1 : 1);
        spawnPoint.z = rb.position.z + UnityEngine.Random.Range(dropPodRangeMin, dropPodRangeMax) * ((randomZOrientation == 0) ? -1 : 1);

        RaycastHit hit;
        Physics.SphereCast(spawnPoint+new Vector3(0,10,0),dropPodSize, Vector3.down, out hit,200,mask,QueryTriggerInteraction.UseGlobal);
        if (hit.collider == null){
            DropPod dp=(Instantiate(dropPod, spawnPoint, Quaternion.identity)).GetComponent<DropPod>();
            dp.data = pData;
            purchases++;
            UpdatePurchasePanel();
        }
        else {//could use method instead
            Debug.Log("help");
            if (hit.collider.gameObject.tag == "DropPod") {
                DropPod dp = (Instantiate(dropPod, spawnPoint+new Vector3(0,5,0), Quaternion.identity)).GetComponent<DropPod>();
                dp.data = pData;
                purchases++;
                UpdatePurchasePanel();
            }
            else
                SpawnDropPod(pData);
        }
    }

    void UpdatePurchasePanel() {
        purchasePanel.text = String.Format("{0} / {1} max purchases", purchases, maxPurchases);
    }

    public void BuyItem(ShopButtonData pData) {
        if (purchases >= maxPurchases)
            return;

        if (pData.cost > money&&(pData.stock!=0))
            return;
        money -= pData.cost;
        pData.stock--;
        SpawnDropPod(pData);
    }



}
