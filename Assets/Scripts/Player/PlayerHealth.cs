using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviourWithPause
{
    [Header("Hp")]
    [SerializeField] float maxHp;
    float currentHP;

    void Start()
    {
        currentHP = maxHp;
    }

    public void TakeDamage(float pDamage)
    {
        currentHP = Mathf.Max(0, currentHP - pDamage);
        if (currentHP == 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.fallenTrees = 0;
    }
}

