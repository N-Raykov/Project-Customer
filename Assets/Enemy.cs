using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{

    [Header("Hp")]
    [SerializeField]float enemyMaxHp;
    float currentHP;

    [Header("References")]
    [SerializeField]GameObject hpBar;
    [SerializeField]RectTransform hpBarTransform;

    [Header("BarLifeline")]
    [SerializeField]float timeBeforeHidingBar;
    float lastHitTime=-100000;

    void Start(){
        hpBar.SetActive(false);
        currentHP = enemyMaxHp;
    }

    void Update(){
        if (Time.time - lastHitTime > timeBeforeHidingBar) {
            hpBar.SetActive(false);
        }
    }

    public void TakeDamage(float pDamage) {
        currentHP = Mathf.Max(0,currentHP-pDamage);
        lastHitTime = Time.time;
        hpBarTransform.localScale=new Vector3(currentHP/enemyMaxHp,1,1);
        hpBar.SetActive(true);
        if (currentHP == 0)
            Die();
    }

    void Die() {
        Destroy(this.gameObject);
    }
}