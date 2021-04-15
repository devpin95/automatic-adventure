using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "")]
public class GameData : ScriptableObject
{
    [SerializeField] private int wave;
    public int Wave
    {
        get => wave;
        set => wave = value;
    }

    public EnemyAttributes[] easyEnemies;
    public EnemyAttributes[] mediumEnemies;
    public EnemyAttributes[] hardEnemies;
    public EnemyAttributes[] bossEnemies;
}
