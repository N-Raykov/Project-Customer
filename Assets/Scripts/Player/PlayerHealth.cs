using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviourWithPause{

    [Header("Hp")]
    [SerializeField] float maxHp;
    //[Header("Hp")]
    [SerializeField] TextMeshProUGUI text;
    //[SerializeField] RectTransform hpBarTransform;
    // We're using an image to "fill" our HP bar, so no need for the transform
    [SerializeField] Image hpImageBar;

    public float currentHP { get; private set; }

    void Start(){
        currentHP = maxHp;
        text.text = string.Format("{0}/{1} HP", currentHP, maxHp);
    }

    public void TakeDamage(float pDamage){
        currentHP = Mathf.Max(0, currentHP - pDamage);
        text.text = string.Format("{0}/{1} HP",currentHP,maxHp);
        //hpBarTransform.localScale = new Vector3(currentHP / maxHp, 1, 1);
        hpImageBar.fillAmount = currentHP / maxHp;

        if (currentHP == 0){
            Die();
        }
    }

    void Die(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.fallenTrees = 0;
    }

    public void AddHp(float pHp) {
        currentHP = Mathf.Min(currentHP + pHp, maxHp);
        text.text = string.Format("{0}/{1} HP", currentHP, maxHp);
        //hpBarTransform.localScale = new Vector3(currentHP / maxHp, 1, 1);
        hpImageBar.fillAmount = currentHP / maxHp;
    }
}

