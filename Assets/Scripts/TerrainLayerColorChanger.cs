using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainLayerColorChanger : MonoBehaviourWithPause
{

    [SerializeField] MoodHandler moodHandler;
    [SerializeField] int grassSlot;
    [Range(0.0f,1.0f)] [SerializeField] float colorGradientChooser;
    private Terrain terrain;

    private void Start()
    {
        terrain = GetComponent<Terrain>();
    }

    // Update is called once per frame
    protected override void UpdateWithPause()
    {
        terrain.terrainData.terrainLayers[grassSlot].diffuseRemapMax = Color.Lerp(moodHandler.start_MainColor, moodHandler.start_SecColor, colorGradientChooser);
    }
}
