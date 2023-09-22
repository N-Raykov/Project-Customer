using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{

    [Header("Abilities")]

    [SerializeField] Image abilityIcon;
    [SerializeField] PlayerAbility playerAbility;
    float CDduration;

    [SerializeField] Image robotSummonIcon;
    SpawnRobot spawnRobot;

    private void Start()
    {
        spawnRobot = FindObjectOfType<SpawnRobot>();
    }

    void Update()
    {
        abilityIcon.fillAmount += 1.0f / playerAbility.abilityCD * Time.deltaTime;

        robotSummonIcon.fillAmount += 1.0f / spawnRobot.spawnCooldown * Time.deltaTime;

        if (spawnRobot.cooldownTimer == 0)
        {
            robotSummonIcon.fillAmount = 1f;
        }
    }

    public void CastAbility()
    {
        abilityIcon.fillAmount = 0;
    }

    public void SpawnRobotUI()
    {
        robotSummonIcon.fillAmount = 0;
    }
}
