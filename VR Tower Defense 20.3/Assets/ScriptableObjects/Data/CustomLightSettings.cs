using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Lighting Preset")]
public class CustomLightSettings : ScriptableObject
{
    // [Header("Fog Settings")]
    public bool fog;
    public float fogDensity;
    public Color fogColor;

    // [Header("Skybox Settings")]
    public Material skybox;
    [FormerlySerializedAs("skyboxIntensity")] public float skyboxExposure = 1;
    [Range(0, 360)] public float skyboxRotation;

    // [Header("Night Settings")]
    public bool isNight;

    // [Header("Main Direction Light Settings")]
    public float mainLightIntensity = 1;
    public Vector3 mainLightRotation;
    
    // [Header("Environment Lighting Settings")]
    public float ambientLightIntensity = 1;
}


[CustomEditor(typeof(CustomLightSettings))]
public class CustomLightSettingsEditor : Editor
{
    private CustomLightSettings settings;
    
    private SerializedProperty m_FogBool;
    private SerializedProperty m_FogDensity;
    private SerializedProperty m_FogColor;

    private SerializedProperty m_Skybox;
    private SerializedProperty m_SkyboxExposure;
    private SerializedProperty m_SkyboxRotation;

    private SerializedProperty m_IsNightBool;
    private SerializedProperty m_MainLightIntensity;
    private SerializedProperty m_MainLightRotation;

    private SerializedProperty m_AmbientLightIntensity;
    
    protected virtual void OnEnable ()
    {
        settings = (CustomLightSettings) target;
        
        m_FogBool = serializedObject.FindProperty("fog");
        m_FogDensity = serializedObject.FindProperty("fogDensity");
        m_FogColor = serializedObject.FindProperty("fogColor");
    
        m_Skybox = serializedObject.FindProperty("skybox");
        m_SkyboxExposure = serializedObject.FindProperty("skyboxExposure");
        m_SkyboxRotation = serializedObject.FindProperty("skyboxRotation");
    
        m_IsNightBool = serializedObject.FindProperty("isNight");
        m_MainLightIntensity = serializedObject.FindProperty("mainLightIntensity");
        m_MainLightRotation = serializedObject.FindProperty("mainLightRotation");
    
        m_AmbientLightIntensity = serializedObject.FindProperty("ambientLightIntensity");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.LabelField("Fog Settings", EditorStyles.boldLabel);
        
        EditorGUILayout.PropertyField(m_FogBool);
        EditorGUI.indentLevel++;

        if (m_FogBool.boolValue)
        {
            settings.fogDensity = EditorGUILayout.FloatField("Density", settings.fogDensity);
            settings.fogColor = EditorGUILayout.ColorField("Color", settings.fogColor);
            // EditorGUILayout.PropertyField("Density", m_FogDensity);
            // EditorGUILayout.PropertyField(m_FogColor);
        }

        EditorGUI.indentLevel--;
        
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Skybox Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_Skybox);
        EditorGUILayout.PropertyField(m_SkyboxExposure);
        EditorGUILayout.PropertyField(m_SkyboxRotation);
        
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(m_IsNightBool);
        
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(m_MainLightRotation);
        EditorGUILayout.PropertyField(m_MainLightIntensity);
        EditorGUILayout.PropertyField(m_AmbientLightIntensity);
        
        serializedObject.ApplyModifiedProperties();
    }
}
