using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(menuName = "ScriptableObjects/Wall Upgrades")]
public class WallUpgrades : ScriptableObject
{
    public GameData gameData;
    
    public int costToRepair100;

    public int defualtWallHealth = 100;
    public int wallUpgradeCount = 0;
    public int[] wallUpgradeHealthValues = {5000, 10000, 15000, 20000, 30000, 50000};
    public int[] wallUpgradeHealthCosts = {200, 300, 400, 500, 600, 700};
    public int numUpgrades = 6;

    public UpgradeCard wallRepair;
    public UpgradeCard wallMaxHealth;

    public void UpgradeWallMaxHealth(UpgradeCard info)
    {
        Debug.Log("U[pgrade max health");
        if (wallUpgradeCount >= numUpgrades) return;
        
        gameData.wallMaxHealth = wallUpgradeHealthValues[wallUpgradeCount];
        gameData.gold -= wallUpgradeHealthCosts[wallUpgradeCount];

        ++wallUpgradeCount;

        UpdateWallMaxHealthCard(info);
    }

    public void HealWall(UpgradeCard info)
    {
        gameData.wallCurrentHealth += 100;
        if (gameData.wallCurrentHealth > gameData.wallMaxHealth) gameData.wallCurrentHealth = gameData.wallMaxHealth;
    }

    private void UpdateWallMaxHealthCard(UpgradeCard info)
    {
        if (wallUpgradeCount >= numUpgrades)
        {
            info.upgradeCost = 0;
            info.maxUpgradeReached = true;
            info.buttonInstance?.gameObject.SetActive(false);
        }
        else
        {
            info.upgradeCost = wallUpgradeHealthCosts[wallUpgradeCount];
        }
    }

    public void UpdateWallCurrentHealthCard(UpgradeCard info)
    {
        if (gameData.wallCurrentHealth == gameData.wallMaxHealth) info.maxUpgradeReached = true;
    }

    public void ResetObject()
    {
        wallUpgradeCount = 0;
        
        gameData.wallCurrentHealth = defualtWallHealth;
        gameData.wallMaxHealth = defualtWallHealth;
        
        wallMaxHealth.upgradeCost = wallUpgradeHealthValues[0];
        wallMaxHealth.maxUpgradeReached = false;
    }
}
