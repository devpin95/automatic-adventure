using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MidStintUIManager : MonoBehaviour
{
    public GameData gameData;

    // [Header("Main GUI")]
    // public TextMeshProUGUI goldAmount;
    // public TextMeshProUGUI waveStint;
    // public TextMeshProUGUI wallIntegrity;
    //
    // [Header("Wall Upgrades")]
    // public WallUpgrades wallUpgrades;
    // [Space(10)]
    // public TextMeshProUGUI nextWallUpgrade;
    // public TextMeshProUGUI nextWallUpgradeCost;
    //
    // [Header("Machinegun Upgrades")] 
    // public MachineGunUpgrades machineGunUpgrades;
    // public TextMeshProUGUI nextMachinegunBulletVelocity;
    // public TextMeshProUGUI nextMachinegunBulletVelocityCost;
    // [Space(10)]
    // public TextMeshProUGUI nextMachinegunRotationSpeed;
    // public TextMeshProUGUI nextMachinegunRotationSpeedCost;
    // [Space(10)]
    // public TextMeshProUGUI nextMachinegunDamage;
    // public TextMeshProUGUI nextMachinegunDamageCost;
    //
    // [Header("Rocket Launcher Upgrades")] 
    // public RocketLauncherUpgrades rocketLauncherUpgrades;
    // public TextMeshProUGUI nextRocketLauncherDamage;
    // public TextMeshProUGUI nextRocketLauncherDamageCost;
    // [Space(10)]
    // public TextMeshProUGUI nextRocketLauncherExplosionRadius;
    // public TextMeshProUGUI nextRocketLauncherExplosionRadiusCost;
    // [Space(10)]
    // public TextMeshProUGUI nextRocketLauncherVelocity;
    // public TextMeshProUGUI nextRocketLauncherVelocityCost;
    // [Space(10)]
    // public TextMeshProUGUI nextRocketLauncherReloadSpeed;
    // public TextMeshProUGUI nextRocketLauncherReloadSpeedCost;
    // [Space(10)]
    // public TextMeshProUGUI rocketLauncherTwoRocketUpgradeCost;
    // public Button rocketLauncherTwoRocketUpgradeButton;
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //     UpdateGoldUI();
    //     UpdateWallHealthUI();
    //     UpdateWallUpgradesUI();
    //     UpdateMachinegunUpgrades();
    //     UpdateRocketLauncherUpgrades();
    //
    //     int wave = gameData.Wave;
    //     int nextStintEnd = wave + 9;
    //
    //     waveStint.text = wave.ToString() + " - " + nextStintEnd.ToString();
    // }

    public void ContinueButton()
    {
        LevelChanger.Instance.FadeToLevel(0);
    }

    // public void RepairWall()
    // {
    //     if (gameData.wallCurrentHealth == gameData.wallMaxHealth) return;
    //     if (gameData.gold < wallUpgrades.costToRepair100) return;
    //
    //     gameData.gold -= wallUpgrades.costToRepair100;
    //     gameData.wallCurrentHealth += 100;
    //
    //     if (gameData.wallCurrentHealth > gameData.wallMaxHealth)
    //     {
    //         float diff = gameData.wallCurrentHealth - gameData.wallMaxHealth;
    //         gameData.gold += (int) diff;
    //         gameData.wallCurrentHealth = gameData.wallMaxHealth;
    //     }
    //     
    //     UpdateGoldUI();
    //     UpdateWallHealthUI();
    // }
    //
    // public void UpgradeWallHealth()
    // {
    //     if (wallUpgrades.wallUpgradeCount >= wallUpgrades.wallUpgradeHealthValues.Length - 1) return;
    //     
    //     ++wallUpgrades.wallUpgradeCount;
    //
    //     int cost = wallUpgrades.wallUpgradeHealthCosts[wallUpgrades.wallUpgradeCount];
    //     int newHealth = wallUpgrades.wallUpgradeHealthValues[wallUpgrades.wallUpgradeCount];
    //
    //     if (gameData.gold >= cost)
    //     {
    //         gameData.gold -= cost;
    //         UpdateGoldUI();
    //
    //         gameData.wallMaxHealth = newHealth;
    //         gameData.wallCurrentHealth = newHealth;
    //         UpdateWallHealthUI();
    //     }
    // }
    //
    // private void UpdateGoldUI()
    // {
    //     goldAmount.text = gameData.gold.ToString();
    // }
    //
    // private void UpdateWallHealthUI()
    // {
    //     wallIntegrity.text = gameData.wallCurrentHealth.ToString() + " / " + gameData.wallMaxHealth;
    // }
    //
    // private void UpdateWallUpgradesUI()
    // {
    //     if (wallUpgrades.wallUpgradeCount + 1 < wallUpgrades.numUpgrades)
    //     {
    //         nextWallUpgradeCost.text = wallUpgrades.wallUpgradeHealthCosts[wallUpgrades.wallUpgradeCount + 1].ToString() + " gold";
    //         nextWallUpgrade.text = wallUpgrades.wallUpgradeHealthValues[wallUpgrades.wallUpgradeCount + 1].ToString() + " health";
    //     }
    //     else
    //     {
    //         nextWallUpgradeCost.text = "--";
    //         nextWallUpgrade.text = "MAX";
    //     }
    // }
    //
    // private void UpdateMachinegunUpgrades()
    // {
    //     int cost = machineGunUpgrades.GetNextBulletVelocityUpgradeCost();
    //     if (cost == 0)
    //     {
    //         nextMachinegunBulletVelocityCost.text = "--";
    //         nextMachinegunBulletVelocity.text = "MAX";
    //     }
    //     else nextMachinegunBulletVelocityCost.text = cost.ToString();
    //
    //     cost = machineGunUpgrades.GetNextBulletDamageUpgradeCost();
    //     if (cost == 0)
    //     {
    //         nextMachinegunDamageCost.text = "--";
    //         nextMachinegunDamage.text = "MAX";
    //     }
    //     else nextMachinegunDamageCost.text = cost.ToString();
    //
    //     cost = machineGunUpgrades.GetNextRotationSpeedUpgradeCost();
    //     if (cost == 0)
    //     {
    //         nextMachinegunRotationSpeedCost.text = "--";
    //         nextMachinegunRotationSpeed.text = "MAX";
    //     }
    //     else nextMachinegunRotationSpeedCost.text = cost.ToString();
    // }
    //
    // public void UpgradeBulletVelocity()
    // {
    //     int cost = machineGunUpgrades.UpgradeBulletVelocity();
    //     gameData.gold -= cost;
    //     UpdateGoldUI();
    //     UpdateMachinegunUpgrades();
    // }
    //
    // public void UpgradeBulletDamage()
    // {
    //     int cost = machineGunUpgrades.UpgradeDamage();
    //     gameData.gold -= cost;
    //     UpdateGoldUI();
    //     UpdateMachinegunUpgrades();
    // }
    //
    // public void UpgradeRotationSpeed()
    // {
    //     int cost = machineGunUpgrades.UpgradeRotationSpeed();
    //     gameData.gold -= cost;
    //     UpdateGoldUI();
    //     UpdateMachinegunUpgrades();
    // }
    //
    // private void UpdateRocketLauncherUpgrades()
    // {
    //     int cost = rocketLauncherUpgrades.GetNextVelocityUpgrade();
    //     if (cost == 0)
    //     {
    //         nextRocketLauncherVelocity.text = "--";
    //         nextRocketLauncherVelocityCost.text = "MAX";
    //     }
    //     else nextRocketLauncherVelocityCost.text = cost.ToString();
    //
    //     cost = rocketLauncherUpgrades.GetNextDamageUpgrade();
    //     if (cost == 0)
    //     {
    //         nextRocketLauncherDamage.text = "--";
    //         nextRocketLauncherDamageCost.text = "MAX";
    //     }
    //     else nextRocketLauncherDamageCost.text = cost.ToString();
    //
    //     cost = rocketLauncherUpgrades.GetNextExplosionRadiusUpgrade();
    //     if (cost == 0)
    //     {
    //         nextRocketLauncherExplosionRadius.text = "--";
    //         nextRocketLauncherExplosionRadiusCost.text = "MAX";
    //     }
    //     else nextRocketLauncherExplosionRadiusCost.text = cost.ToString();
    //     
    //     cost = rocketLauncherUpgrades.GetNextReloadSpeedUpgrade();
    //     if (cost == 0)
    //     {
    //         nextRocketLauncherReloadSpeed.text = "--";
    //         nextRocketLauncherReloadSpeedCost.text = "MAX";
    //     }
    //     else nextRocketLauncherReloadSpeedCost.text = cost.ToString();
    //
    //
    //     if (rocketLauncherUpgrades.twoRocketUpgradePurchased)
    //     {
    //         rocketLauncherTwoRocketUpgradeCost.enabled = false;
    //         rocketLauncherTwoRocketUpgradeButton.enabled = false;
    //     }
    //     else
    //     {
    //         cost = rocketLauncherUpgrades.GetTwoRocketUpgradeCost();
    //         rocketLauncherTwoRocketUpgradeCost.text = cost.ToString();
    //     }
    //
    // }
    //
    // public void UpgradeRocketLauncherDamage()
    // {
    //     int cost = rocketLauncherUpgrades.UpgradeDamage();
    //     gameData.gold -= cost;
    //     UpdateGoldUI();
    //     UpdateRocketLauncherUpgrades();
    // }
    //
    // public void UpgradeRocketLauncherVelocity()
    // {
    //     int cost = rocketLauncherUpgrades.UpgradeVelocity();
    //     gameData.gold -= cost;
    //     UpdateGoldUI();
    //     UpdateRocketLauncherUpgrades();
    // }
    //
    // public void UpgradeRocketLauncherExplosionRadius()
    // {
    //     int cost = rocketLauncherUpgrades.UpgradeRadius();
    //     gameData.gold -= cost;
    //     UpdateGoldUI();
    //     UpdateRocketLauncherUpgrades();
    // }
    //
    // public void UpgradeRocketLauncherReloadSpeed()
    // {
    //     int cost = rocketLauncherUpgrades.UpgradeReloadSpeed();
    //     gameData.gold -= cost;
    //     UpdateGoldUI();
    //     UpdateRocketLauncherUpgrades();
    // }
    //
    // public void UpgradeTwoRockets()
    // {
    //     int cost = rocketLauncherUpgrades.UpgradeTwoRockets();
    //     gameData.gold -= cost;
    //     UpdateGoldUI();
    //     UpdateRocketLauncherUpgrades();
    // }
}
