using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveWarningUI : MonoBehaviour
{
    [SerializeField] GameObject warningHolder;

    void ShowPrompt()
    {
        warningHolder.SetActive(true);
        Invoke("HidePrompt", 5);
    }

    void HidePrompt()
    {
        warningHolder.SetActive(false);
    }

    private void OnEnable()
    {
        EnemySpawner.onWaveEvent += ShowPrompt;
    }

    private void OnDisable()
    {
        EnemySpawner.onWaveEvent -= ShowPrompt;
    }
}
