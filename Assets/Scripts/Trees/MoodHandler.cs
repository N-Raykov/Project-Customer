using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class MoodHandler : MonoBehaviourWithPause
{

    //[SerializeField] List<Material> leafMaterialList;

    [SerializeField] [Range(0.0f,1.0f)] float globalDegradation = 0.0f;
    [SerializeField] MusicHandler musicHandler;
    [SerializeField] Volume pp_volume;

    [Header("Sunlight")]
    [SerializeField] private Light sunLight;
    [SerializeField] private Color start_SunColor;
    [SerializeField] private Color end_SunColor;

    [Header("Skybox")]
    [SerializeField] private Material skyBoxMat;
    [SerializeField] private Color start_SkyColor;
    [SerializeField] private Color start_GroundColor;

    [SerializeField] private Color end_SkyColor;
    [SerializeField] private Color end_GroundColor;

    [Header("Leaf Colors")]
    [SerializeField] private Color start_MainColor;
    [SerializeField] private Color start_SecColor;

    [SerializeField] private Color end_MainColor;
    [SerializeField] private Color end_SecColor;

    // Update is called once per frame
    protected override void UpdateWithPause()
    {
        ////FOR TESTING ONLY!!!
        //if (Application.isPlaying)
        //{
        //    //    globalDegradation = (Mathf.Sin(-0.5f*Mathf.PI + (Time.realtimeSinceStartup / 10))/2) +0.5f;
        //    globalDegradation = (float)(GameManager.fallenTrees) / 6;
        //}

        //globalDegradation = (float) (6 - GameManager.fallenTrees) / 6;

        musicHandler.musicTransitionProgress = globalDegradation;
        float degradeLerp = Mathf.InverseLerp(0.6f, 1.0f, globalDegradation);
        float ColorLerp = Mathf.InverseLerp(0f, 0.5f, globalDegradation);

        sunLight.color = Color.Lerp(start_SunColor,end_SunColor,ColorLerp);
        skyBoxMat.SetColor("_SkyTint", Color.Lerp(start_SkyColor, end_SkyColor, degradeLerp));
        skyBoxMat.SetColor("_GroundColor", Color.Lerp(start_GroundColor, end_GroundColor, degradeLerp));

        Shader.SetGlobalFloat("_DegradationProgress", degradeLerp);
        Shader.SetGlobalColor("_Light_Color", Color.Lerp(start_MainColor, end_MainColor, ColorLerp));
        Shader.SetGlobalColor("_Dark_Color", Color.Lerp(start_SecColor, end_SecColor, ColorLerp));

        ColorAdjustments col_Adj;
        Vignette vignette;
        pp_volume.profile.TryGet(out col_Adj);
        pp_volume.profile.TryGet(out vignette);

        col_Adj.saturation.value = Mathf.Lerp(15, -85, globalDegradation);
        vignette.intensity.value = Mathf.Lerp(0.0f, 0.4f, globalDegradation);
    }
}
