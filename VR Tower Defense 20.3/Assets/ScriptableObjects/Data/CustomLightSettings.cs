using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CustomLightSettings
{
    [Header("Fog Settings")]
    public bool fog;
    public float fogDensity;
    public Color fogColor;

    [Header("Skybox Settings")]
    public Material skybox;
    [FormerlySerializedAs("skyboxIntensity")] public float skyboxExposure = 1;
    [Range(0, 360)] public float skyboxRotation;

    [Header("Night Settings")]
    public bool isNight;

    [Header("Main Direction Light Settings")]
    public float mainLightIntensity;
    public Vector3 mainLightRotation;
    
    [Header("Environment Lighting Settings")]
    public float ambientLightIntensity;
}
