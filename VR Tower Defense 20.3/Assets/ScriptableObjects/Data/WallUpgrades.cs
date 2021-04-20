using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Wall Upgrades")]
public class WallUpgrades : ScriptableObject
{
    public int costToRepair100;
    public int wallUpgradeCount = 0;
    public int[] wallUpgradeHealthValues = new int[] {5000, 10000, 20000, 40000, 80000, 160000};
    public int[] wallUpgradeHealthCosts = new int[] {5000, 10000, 20000, 30000, 40000, 50000};
    public int numUpgrades = 6;
}
