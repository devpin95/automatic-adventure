using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MidStintUIManager : MonoBehaviour
{
    public GameData gameData;
    public WallUpgrades wallUpgrades;

    public TextMeshProUGUI goldAmount;
    public TextMeshProUGUI waveStint;
    public TextMeshProUGUI wallIntegrity;

    public TextMeshProUGUI nextWallUpgrade;
    public TextMeshProUGUI nextWallUpgradeCost;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateGoldUI();
        UpdateWallHealthUI();
        UpdateWallUpgradesUI();

        int wave = gameData.Wave;
        int nextStintEnd = wave + 9;

        waveStint.text = wave.ToString() + " - " + nextStintEnd.ToString();
    }

    public void ContinueButton()
    {
        LevelChanger.Instance.FadeToLevel(0);
    }

    public void RepairWall()
    {
        if (gameData.wallCurrentHealth == gameData.wallMaxHealth) return;
        if (gameData.gold < wallUpgrades.costToRepair100) return;

        gameData.gold -= wallUpgrades.costToRepair100;
        gameData.wallCurrentHealth += 100;

        if (gameData.wallCurrentHealth > gameData.wallMaxHealth)
        {
            float diff = gameData.wallCurrentHealth - gameData.wallMaxHealth;
            gameData.gold += (int) diff;
            gameData.wallCurrentHealth = gameData.wallMaxHealth;
        }
        
        UpdateGoldUI();
        UpdateWallHealthUI();
    }

    public void UpgradeWallHealth()
    {
        if (wallUpgrades.wallUpgradeCount >= wallUpgrades.wallUpgradeHealthValues.Length - 1) return;
        
        ++wallUpgrades.wallUpgradeCount;

        int cost = wallUpgrades.wallUpgradeHealthCosts[wallUpgrades.wallUpgradeCount];
        int newHealth = wallUpgrades.wallUpgradeHealthValues[wallUpgrades.wallUpgradeCount];

        if (gameData.gold >= cost)
        {
            gameData.gold -= cost;
            UpdateGoldUI();

            gameData.wallMaxHealth = newHealth;
            gameData.wallCurrentHealth = newHealth;
            UpdateWallHealthUI();
        }
    }

    private void UpdateGoldUI()
    {
        goldAmount.text = gameData.gold.ToString();
    }

    private void UpdateWallHealthUI()
    {
        wallIntegrity.text = gameData.wallCurrentHealth.ToString() + " / " + gameData.wallMaxHealth;
    }

    private void UpdateWallUpgradesUI()
    {
        if (wallUpgrades.wallUpgradeCount + 1 < wallUpgrades.numUpgrades)
        {
            nextWallUpgradeCost.text = wallUpgrades.wallUpgradeHealthCosts[wallUpgrades.wallUpgradeCount + 1].ToString() + " gold";
            nextWallUpgrade.text = wallUpgrades.wallUpgradeHealthValues[wallUpgrades.wallUpgradeCount + 1].ToString() + " health";
        }
        else
        {
            nextWallUpgradeCost.text = "--";
            nextWallUpgrade.text = "MAX";
        }
    }
}
