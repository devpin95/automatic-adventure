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

    // player
    public int currentActiveTower;
    public int gold;
    
    // wall
    public float gameDefaultWallHealth = 50;
    public float wallMaxHealth;
    public float wallCurrentHealth;
    

    public EnemyAttributes[] easyEnemies;
    public EnemyAttributes[] mediumEnemies;
    public EnemyAttributes[] hardEnemies;
    public EnemyAttributes[] bossEnemies;

    public void ResetObject()
    {
        Wave = 0;
        currentActiveTower = 0;
        gold = 0;

        wallMaxHealth = gameDefaultWallHealth;
        wallCurrentHealth = wallMaxHealth;
    }
}
