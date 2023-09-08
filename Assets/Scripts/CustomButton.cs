using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CustomButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] ShopManager shopManager;
    [SerializeField] ShopButtonData buttonSO;
    ShopButtonData SOData;

    void Awake()
    {
        SOData = ScriptableObject.CreateInstance<ShopButtonData>();
        SOData.CopyFrom(buttonSO);
        button.onClick.AddListener(CustomButton_onClick); 
    }

    //Handle the onClick event
    void CustomButton_onClick()
    {
        shopManager.BuyItem(SOData);
    }
}
