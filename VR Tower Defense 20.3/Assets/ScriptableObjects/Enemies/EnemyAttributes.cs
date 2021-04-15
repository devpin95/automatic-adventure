using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Enemies/Attributes")]
public class EnemyAttributes : ScriptableObject
{
    public string enemyName;
    public Vector3 rotation;
    public bool randomRotation;
    public float startingHealth;

    [SerializeField] private GameObject prefab;
    [SerializeField] private int enemyValue;
    [SerializeField] private int waveSpawnModifier;
    [FormerlySerializedAs("waveMod10Modifier")] [SerializeField] private bool countPer10WavesModifier;

    public int WaveSpawnModifier => waveSpawnModifier;
    public GameObject Prefab => prefab;
    public int EnemyValue => enemyValue;
    public bool CountPer10WavesModifier => countPer10WavesModifier;
    
}
