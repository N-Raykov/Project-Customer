using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun",menuName ="Weapon/Gun")]
public class GunData : ScriptableObject{
    [Header("Info")]
    public string gunName;
    [Header("Shooting")]
    public float damage;
    public float range;
    public float bulletSpeed;
    [Header("Reloading")]
    public int ammoCapacity;
    public float shotCooldown;
    public float reloadTime;
    [Header("Spread")]
    public float spreadFactorX;
    public float spreadFactorY;
    public float spreadPercentageMultiplier;
    public float spreadPercentageMultiplierAim;
    public float spreadDecreaseRate;
    [Header("Recoil")]
    public Vector3 recoilHipFire;
    public Vector3 recoilAim;
    public float returnSpeed;
    [Header("ZoomIn")]
    public Vector3 targetPosition;
    public float zoomInDuration;
    public float zoomInFactor;
}
