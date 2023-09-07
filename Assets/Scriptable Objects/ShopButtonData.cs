using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun",menuName ="Shop/ShopButton")]
public class ShopButtonData : ScriptableObject{
    [Header("Info")]
    public string item;//name
    public string type;//weapons/supplies/abilities
    public int cost;
    public int stock;
    public int amount;
}
