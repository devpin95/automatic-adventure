using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Enemies/Attributes")]
public class EnemyAttributes : ScriptableObject
{
    public enum EnemyType
    {  
        Basic,
        Armored
    }
    
    public string enemyName;

    public EnemyType enemyType;
    
    [Space(10)]
    
    public float wallHitDamage;
    public bool randomRotation;
    public Vector3 rotation;

    [Space(10)]
    
    public bool isFixed = false;
    [Tooltip("A fixed spawn area. Center X, Center Z, width (x direction), height (z direction)")]
    public Vector4 fixedPosition;

    [Space(10)]
    
    [Tooltip("Enemy Pool set in pool startup. Pass this object to the script that creates the pool.")]
    public ObjectPool pool;
    [Tooltip("If true, the SpawnManager will wait for this object to be killed before moving on to the next wave.")]
    public bool countAsEnemy;
    
    [Space(10)]

    [SerializeField] private GameObject prefab;
    [SerializeField] private int enemyValue;
    public float startingHealth;

    [Space(10)]
    [Tooltip("Width x Depth for spawning in a 2d grid")]
    public Vector2 spawnGridDimensions;
    
    public GameObject Prefab => prefab;
    public int EnemyValue => enemyValue;
}
