using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName ="Gun",menuName ="Shop/ShopButton")]
public class ShopButtonData : ScriptableObject{

    [Header("Info")]
    public string item;//name
    public string type;//weapons/supplies/abilities
    public int cost;
    public int stock;
    public int amount;

    public void CopyFrom(ShopButtonData other)
    {
        this.name = other.name;
        this.item = other.item;
        this.type = other.type;
        this.cost = other.cost;
        this.stock = other.stock;
        this.amount = other.amount;
    }
}
