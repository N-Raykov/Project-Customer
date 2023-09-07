using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun",menuName ="Shop/ShopButton")]
public class ShopButtonData : ScriptableObject{
    [Header("Info")]
    public string item;//name
    public string type;//weapon/supplies/ability
    public int cost;
    public int stock;
}
