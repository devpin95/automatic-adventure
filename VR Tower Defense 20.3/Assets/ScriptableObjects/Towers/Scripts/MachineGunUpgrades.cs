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
    
    // Accuracy --------------------------------------------------------------------------------------------------------
    [Header("Accuracy Upgrades")]
    [FormerlySerializedAs("defaultAccuracyModifier")][SerializeField] private float defaultAccuracy;
    [SerializeField] private float bulletAccuracy;
    
    public float BulletAccuracy
    {
        get => bulletAccuracy;
        set => bulletAccuracy = value;
    }
    
    public int bulletAccuracyUpgradeStep = 0;
    public int[] bulletAccuracyUpgradeCosts = {  };
    public float[] bulletAccuracyUpgradeValues = {  };
    // *****************************************************************************************************************

    [Header("Upgrade Cards")] 
    public UpgradeCard damageCard;
    public UpgradeCard velocityCard;
    public UpgradeCard rotationCard;
    public UpgradeCard accuracyCard;
    
    
    public void ResetObject()
    {
        damage = defaultDamage;
        damageUpgradeStep = 0;
        
        towerRotationSpeed = defaultTowerRotationSpeed;
        rotationUpgradeStep = 0;
        
        bulletVelocityModifier = defaultBulletVelocityModifier;
        bulletVelocityUpgradeStep = 0;

        bulletAccuracy = defaultAccuracy;
        bulletAccuracyUpgradeStep = 0;
        
        UpgradeProjectile();
        initialized = false;
        
        damageCard.upgradeCost = damageUpgradeCosts[0];
        damageCard.maxUpgradeReached = false;
        damageCard.buttonInstance = null;
        
        velocityCard.upgradeCost = bulletVelocityUpgradeCosts[0];
        velocityCard.maxUpgradeReached = false;
        velocityCard.buttonInstance = null;
        
        rotationCard.upgradeCost = rotationUpgradeCosts[0];
        rotationCard.maxUpgradeReached = false;
        rotationCard.buttonInstance = null;
        
        accuracyCard.upgradeCost = bulletAccuracyUpgradeCosts[0];
        accuracyCard.maxUpgradeReached = false;
        accuracyCard.buttonInstance = null;
    }

    public void Init()
    {
        if (initialized) return;
        
        damage = defaultDamage;
        towerRotationSpeed = defaultTowerRotationSpeed;
        bulletVelocityModifier = defaultBulletVelocityModifier;
        initialized = true;

        damageCard.upgradeCost = damageUpgradeCosts[0];
        damageCard.getUpgradeValue = GetNextBulletDamageUpgradeValue;
        damageCard.purchase = UpgradeDamage;
        damageCard.updateCard = UpdateDamageCard;

        velocityCard.upgradeCost = bulletVelocityUpgradeCosts[0];
        velocityCard.getUpgradeValue = GetNextBulletVelocityUpgradeValue;
        velocityCard.purchase = UpgradeBulletVelocity;
        velocityCard.updateCard = UpdateVelocityCard;
        
        rotationCard.upgradeCost = rotationUpgradeCosts[0];
        rotationCard.getUpgradeValue = GetNextRotationSpeedUpgradeValue;
        rotationCard.purchase = UpgradeRotationSpeed;
        rotationCard.updateCard = UpdateRotationSpeedCard;

        accuracyCard.upgradeCost = bulletAccuracyUpgradeCosts[0];
        accuracyCard.getUpgradeValue = GetNextBulletAccuracyUpgradeValue;
        accuracyCard.purchase = UpgradeBulletAccuracy;
        accuracyCard.updateCard = UpdateBulletAccuracyCard;
        
    }

    private void UpgradeProjectile()
    {
        bulletAttributes.damage = damage;
    }
    
    // UPGRADE CARD FUNCTION -------------------------------------------------------------------------------------------
    // *****************************************************************************************************************

    public void UpgradeDamage()
    {
        if (damageUpgradeStep > damageUpgradeCosts.Length - 1) return;
        
        if (gameData.gold >= damageUpgradeCosts[damageUpgradeStep])
        {
            Damage = damageUpgradeValues[damageUpgradeStep];
            gameData.gold -= damageUpgradeCosts[damageUpgradeStep];
            UpgradeProjectile();
            ++damageUpgradeStep;
        }
        
        UpdateDamageCard();
    }

    public void UpgradeRotationSpeed()
    {
        if (rotationUpgradeStep > rotationUpgradeCosts.Length - 1) return;
        if (gameData.gold >= rotationUpgradeCosts[rotationUpgradeStep])
        {
            TowerRotationSpeed = rotationUpgradeValues[rotationUpgradeStep];
            gameData.gold -= rotationUpgradeCosts[rotationUpgradeStep];
            ++rotationUpgradeStep;
        }
        
        UpdateRotationSpeedCard();
    }

    public void UpgradeBulletVelocity()
    {
        if (bulletVelocityUpgradeStep > bulletVelocityUpgradeCosts.Length - 1) return;
        
        if (gameData.gold >= bulletVelocityUpgradeCosts[bulletVelocityUpgradeStep])
        {
            BulletVelocityModifier = bulletVelocityUpgradeValues[bulletVelocityUpgradeStep];
            gameData.gold -= bulletVelocityUpgradeCosts[bulletVelocityUpgradeStep];

            ++bulletVelocityUpgradeStep;
        }
        
        UpdateVelocityCard();
    }
    
    public void UpgradeBulletAccuracy()
    {
        if (bulletAccuracyUpgradeStep > bulletAccuracyUpgradeCosts.Length - 1) return;
        if (gameData.gold >= bulletAccuracyUpgradeCosts[bulletAccuracyUpgradeStep])
        {
            bulletAccuracy = bulletAccuracyUpgradeValues[bulletAccuracyUpgradeStep];
            gameData.gold -= bulletAccuracyUpgradeCosts[bulletAccuracyUpgradeStep];
            ++bulletAccuracyUpgradeStep;
        }
        
        UpdateBulletAccuracyCard();
    }
    
    // *****************************************************************************************************************
    // *****************************************************************************************************************
    
    
    // NEXT UPGRADE FUNCTION -------------------------------------------------------------------------------------------
    // *****************************************************************************************************************

    public float GetNextBulletVelocityUpgradeValue()
    {
        if (bulletVelocityUpgradeStep >= bulletVelocityUpgradeValues.Length) return 0;
        else return bulletVelocityUpgradeValues[bulletVelocityUpgradeStep];
    }
    
    public float GetNextRotationSpeedUpgradeValue()
    {
        if (rotationUpgradeStep >= rotationUpgradeValues.Length) return 0;
        else return rotationUpgradeValues[rotationUpgradeStep];
    }
    
    public float GetNextBulletDamageUpgradeValue()
    {
        if (damageUpgradeStep >= damageUpgradeValues.Length) return 0;
        else return damageUpgradeValues[damageUpgradeStep];
    }
    
    public float GetNextBulletAccuracyUpgradeValue()
    {
        if (bulletAccuracyUpgradeStep >= bulletAccuracyUpgradeValues.Length) return 0;
        else return bulletAccuracyUpgradeValues[bulletAccuracyUpgradeStep];
    }
    
    // *****************************************************************************************************************
    // *****************************************************************************************************************
    
    
    // UPDATE CARD FUNCTION --------------------------------------------------------------------------------------------
    // *****************************************************************************************************************

    public void UpdateDamageCard()
    {
        if (damageUpgradeStep >= damageUpgradeValues.Length)
        {
            damageCard.maxUpgradeReached = true;
            damageCard.upgradeCost = 0;
            damageCard.buttonInstance.gameObject.SetActive(false);
        }
        else
        {
            damageCard.upgradeCost = damageUpgradeCosts[damageUpgradeStep];
        }
    }

    public void UpdateVelocityCard()
    {
        if (bulletVelocityUpgradeStep >= bulletVelocityUpgradeValues.Length)
        {
            velocityCard.maxUpgradeReached = true;
            velocityCard.upgradeCost = 0;
            velocityCard.buttonInstance.gameObject.SetActive(false);
        }
        else
        {
            velocityCard.upgradeCost = bulletVelocityUpgradeCosts[bulletVelocityUpgradeStep];
        }
    }

    public void UpdateRotationSpeedCard()
    {
        if (rotationUpgradeStep >= rotationUpgradeValues.Length)
        {
            rotationCard.maxUpgradeReached = true;
            rotationCard.upgradeCost = 0;
            rotationCard.buttonInstance.gameObject.SetActive(false);
        }
        else
        {
            rotationCard.upgradeCost = rotationUpgradeCosts[rotationUpgradeStep];
        }
    }
    
    public void UpdateBulletAccuracyCard()
    {
        if (bulletAccuracyUpgradeStep >= bulletAccuracyUpgradeValues.Length)
        {
            accuracyCard.maxUpgradeReached = true;
            accuracyCard.upgradeCost = 0;
            accuracyCard.buttonInstance.gameObject.SetActive(false);
        }
        else
        {
            accuracyCard.upgradeCost = bulletAccuracyUpgradeCosts[bulletAccuracyUpgradeStep];
        }
    }
    
    // *****************************************************************************************************************
    // *****************************************************************************************************************
}
