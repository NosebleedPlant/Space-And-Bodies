using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RM_Controler : MonoBehaviour
{
    // [SerializeField] private Transform light;
    // [SerializeField] private UniversalRendererData rendererData = null;
    // private string featureName = "RayMarcher";
    //
    // private void Start()
    // {
    //     UpdateCamera();
    // }
    //
    // private bool TryGetFeature(out ScriptableRendererFeature feature)
    // {
    //     feature = rendererData.rendererFeatures.Where((f) => f.name == featureName).FirstOrDefault();
    //     return feature != null;
    // }
    //
    // private void UpdateCamera()
    // {
    //     if (TryGetFeature(out var feature))
    //     {
    //         var raymarcher = feature as RayMarcher;
    //         raymarcher.settings.lightTransform = light;
    //     }
    // }
}
