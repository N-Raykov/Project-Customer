using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetShopData : MonoBehaviourWithPause{

    [SerializeField] ShopButtonData data;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] TextMeshProUGUI stock;

    void Awake(){
        ignorePausedState = true;
        price.text = string.Format("{0}$",data.cost);
    }

    protected override void UpdateWithPause(){
        if (data.stock < 0){
            stock.text = "Infinite";
        }
        else {
            stock.text = string.Format("{0} Left",data.stock);
        }
    }

}
