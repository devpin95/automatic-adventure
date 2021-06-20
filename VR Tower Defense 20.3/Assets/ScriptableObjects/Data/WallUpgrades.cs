using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/Wall Upgrades")]
public class WallUpgrades : ScriptableObject
{
    public GameData gameData;
    
    [FormerlySerializedAs("defaultCostToRepair")]
    [Header("Repair options")]
    [FormerlySerializedAs("costToRepair100")] public int defaultCostToRepair10Points;
    public int defaultRepairPoints = 10;
    public int upgradedRepairPoints = 100;
    private int _currentRepairPoints = 10;

    [Header("Wall Max Health Upgrades")]
    [FormerlySerializedAs("defualtWallHealth")] public int defaultWallHealth = 100;
    public int wallUpgradeCount = 0;
    public int numUpgrades = 6;
    public int[] wallUpgradeHealthValues = {5000, 10000, 15000, 20000, 30000, 50000};
    public int[] wallUpgradeHealthCosts = {200, 300, 400, 500, 600, 700};

    [Header("Upgrade Cards")]
    public UpgradeCard wallRepair;
    public UpgradeCard wallMaxHealth;

    public void UpgradeWallMaxHealth()
    {
        Debug.Log("U[pgrade max health");
        if (wallUpgradeCount >= numUpgrades) return;
        
        gameData.wallMaxHealth = wallUpgradeHealthValues[wallUpgradeCount];
        gameData.gold -= wallUpgradeHealthCosts[wallUpgradeCount];

        ++wallUpgradeCount;

        if (wallUpgradeCount > 3) _currentRepairPoints = upgradedRepairPoints;

        UpdateWallMaxHealthCard();
        UpdateWallCurrentHealthCard();
    }

    public void HealWall()
    {
        gameData.wallCurrentHealth += _currentRepairPoints;
        if (gameData.wallCurrentHealth > gameData.wallMaxHealth) gameData.wallCurrentHealth = gameData.wallMaxHealth;
        
        int cost = (_currentRepairPoints / 10) * defaultCostToRepair10Points;
        gameData.gold -= cost;
        
        // Debug.Log("HEALING " + _currentRepairPoints + " WALL POINTS FOR " + cost + " CREDITS");

        UpdateWallCurrentHealthCard();
    }

    public void UpdateWallMaxHealthCard()
    {
        if (wallUpgradeCount >= numUpgrades)
        {
            wallMaxHealth.upgradeCost = 0;
            wallMaxHealth.maxUpgradeReached = true;
            wallMaxHealth.buttonInstance?.gameObject.SetActive(false);
        }
        else
        {
            wallMaxHealth.upgradeCost = wallUpgradeHealthCosts[wallUpgradeCount];
        }
    }

    public void UpdateWallCurrentHealthCard()
    {
        if (gameData.wallCurrentHealth == gameData.wallMaxHealth)
        {
            wallRepair.buttonInstance?.gameObject.SetActive(false);
            wallRepair.maxUpgradeReached = true;
        }
        else
        {
            wallRepair.buttonInstance?.gameObject.SetActive(true);
            wallRepair.maxUpgradeReached = false;
        }
    }

    public void ResetObject()
    {
        wallUpgradeCount = 0;
        
        gameData.wallCurrentHealth = defaultWallHealth;
        gameData.wallMaxHealth = defaultWallHealth;

        // wallRepair.getUpgradeValue = null;
        // wallRepair.updateCard = null;
        wallRepair.maxUpgradeReached = false;
        wallRepair.buttonInstance = null;
        
        // wallMaxHealth.getUpgradeValue = null;
        // wallMaxHealth.updateCard = null;
        wallMaxHealth.upgradeCost = wallUpgradeHealthValues[0];
        wallMaxHealth.maxUpgradeReached = false;
        wallMaxHealth.buttonInstance = null;

        _currentRepairPoints = defaultRepairPoints;
    }

    public void Init()
    {
        wallRepair.getUpgradeValue = GetNextHealthUpgradeValue;
        wallRepair.updateCard = UpdateWallCurrentHealthCard;
        wallRepair.purchase = HealWall;
        
        wallMaxHealth.getUpgradeValue = GetNextMaxHealthUpgradeValue;
        wallMaxHealth.updateCard = UpdateWallMaxHealthCard;
        wallMaxHealth.purchase = UpgradeWallMaxHealth;
    }

    public float GetNextMaxHealthUpgradeValue()
    {
        if (wallUpgradeCount + 1 >= numUpgrades) return 0;
        return wallUpgradeHealthValues[wallUpgradeCount + 1] - gameData.wallMaxHealth;
    }

    public float GetNextHealthUpgradeValue()
    {
        float healVal = _currentRepairPoints;
        
        float dif = gameData.wallMaxHealth - gameData.wallCurrentHealth;

        if (dif < _currentRepairPoints)
        {
            healVal = (int)dif;
        } else if (dif == 0)
        {
            healVal = 0;
        }
        
        return healVal;
    }
}
