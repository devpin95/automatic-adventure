using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Towers/Machinegun/Upgrades")]
public class MachineGunUpgrades : ScriptableObject
{
    public bool initialized = false;

    public ProjectileAttributes bulletAttributes;
    public GameData gameData;
    
    // DAMAGE ----------------------------------------------------------------------------------------------------------
    [Header("Damage Upgrades")]
    [SerializeField] private int defaultDamage = 1;
    [SerializeField] private float damage;

    public float Damage
    {
        get => damage;
        set { damage = value; UpgradeProjectile(); }
    }
    public int damageUpgradeStep = 0;
    public int[] damageUpgradeCosts = { 5000, 10000, 20000, 40000};
    public float[] damageUpgradeValues = { 2, 4, 8, 16 };
    // *****************************************************************************************************************

    
    // ROTATION --------------------------------------------------------------------------------------------------------
    [Header("Rotation Upgrades")]
    [SerializeField] private float defaultTowerRotationSpeed = 12f;
    [SerializeField] private float towerRotationSpeed;
    public float TowerRotationSpeed
    {
        get => towerRotationSpeed;
        set => towerRotationSpeed = value;
    }
    
    public int rotationUpgradeStep = 0;
    public int[] rotationUpgradeCosts = { 1000, 2000 };
    public int[] rotationUpgradeValues = { 15, 17 };
    // *****************************************************************************************************************

    
    // VELOCITY --------------------------------------------------------------------------------------------------------
    [Header("Velocity Upgrades")]
    [SerializeField] private float defaultBulletVelocityModifier = 15;
    [SerializeField] private float bulletVelocityModifier;
    public float BulletVelocityModifier
    {
        get => bulletVelocityModifier;
        set => bulletVelocityModifier = value;
    }
    
    [FormerlySerializedAs("bulletvelocityUpgradeStep")] public int bulletVelocityUpgradeStep = 0;
    public int[] bulletVelocityUpgradeCosts = { 1000, 2000 };
    public int[] bulletVelocityUpgradeValues = { 16, 17 };
    // *****************************************************************************************************************
    
    
    public void ResetObject()
    {
        damage = defaultDamage;
        damageUpgradeStep = 0;
        
        towerRotationSpeed = defaultTowerRotationSpeed;
        rotationUpgradeStep = 0;
        
        bulletVelocityModifier = defaultBulletVelocityModifier;
        bulletVelocityUpgradeStep = 0;
        
        UpgradeProjectile();
        initialized = false;
    }

    public void Init()
    {
        if (initialized) return;
        
        damage = defaultDamage;
        towerRotationSpeed = defaultTowerRotationSpeed;
        bulletVelocityModifier = defaultBulletVelocityModifier;
        initialized = true;
    }

    private void UpgradeProjectile()
    {
        bulletAttributes.damage = damage;
    }

    public int UpgradeDamage()
    {
        if (damageUpgradeStep > damageUpgradeCosts.Length - 1) return 0;
        if (gameData.gold >= damageUpgradeCosts[damageUpgradeStep])
        {
            Damage = damageUpgradeValues[damageUpgradeStep];
            UpgradeProjectile();
            int cost = damageUpgradeCosts[damageUpgradeStep];
            ++damageUpgradeStep;
            return cost;
        }
        
        return 0;
    }

    public int UpgradeRotationSpeed()
    {
        if (rotationUpgradeStep > rotationUpgradeCosts.Length - 1) return 0;
        if (gameData.gold >= rotationUpgradeCosts[rotationUpgradeStep])
        {

            int cost = rotationUpgradeCosts[rotationUpgradeStep];
            TowerRotationSpeed = rotationUpgradeValues[rotationUpgradeStep];
            ++rotationUpgradeStep;
            return cost;
        }
        
        return 0;
    }

    public int UpgradeBulletVelocity()
    {
        if (bulletVelocityUpgradeStep > bulletVelocityUpgradeCosts.Length - 1) return 0;
        
        if (gameData.gold >= bulletVelocityUpgradeCosts[bulletVelocityUpgradeStep])
        {
            BulletVelocityModifier = bulletVelocityUpgradeValues[bulletVelocityUpgradeStep];
            Debug.Log("NEW VELOCITY: " + BulletVelocityModifier);

            
            int cost = bulletVelocityUpgradeCosts[bulletVelocityUpgradeStep];
            ++bulletVelocityUpgradeStep;
            return cost;
        }

        return 0;
    }

    public int GetNextBulletVelocityUpgradeCost()
    {
        if (bulletVelocityUpgradeStep >= bulletVelocityUpgradeCosts.Length) return 0;
        else return bulletVelocityUpgradeCosts[bulletVelocityUpgradeStep];
    }
    
    public int GetNextRotationSpeedUpgradeCost()
    {
        if (rotationUpgradeStep >= rotationUpgradeCosts.Length) return 0;
        else return rotationUpgradeCosts[rotationUpgradeStep];
    }
    
    public int GetNextBulletDamageUpgradeCost()
    {
        if (damageUpgradeStep >= damageUpgradeCosts.Length) return 0;
        else return damageUpgradeCosts[damageUpgradeStep];
    }
}
