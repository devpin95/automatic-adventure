using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Towers/Rocket Launcher/Upgrades")]
public class RocketLauncherUpgrades : ScriptableObject
{
    public bool initialized;

    public ProjectileAttributes rocketAttributes;
    public GameData gameData;

    
    // DAMAGE ----------------------------------------------------------------------------------------------------------
    [Header("Rocket Damage Upgrades")]
    public float defaultDamage;
    private float damage;
    public float Damage
    {
        get => damage;
        set => damage = value;
    }
    [SerializeField] private int damageUpgradeStep = 0;
    public float[] damageUpgradeValues = {150, 200, 300};
    public int[] damageUpgradeCosts = {5000, 15000, 25000};
    // *****************************************************************************************************************
    
    
    // EXPLOSION RADIUS ------------------------------------------------------------------------------------------------
    [Header("Explosion Radius Upgrades")]
    [FormerlySerializedAs("defaultRadius")] public float defaultExplosionRadius = 5;
    private float explosionRadius;
    public float ExplosionRadius
    {
        get => explosionRadius;
        set => explosionRadius = value;
    }
    [SerializeField] private int radiusUpgradeStep = 0;
    public float[] radiusUpgradeValues = { 6, 10, 15, 20 };
    public int[] radiusUpgradeCosts = { 1000, 5000, 10000, 20000 };
    // *****************************************************************************************************************
    
    
    // VELOCITY --------------------------------------------------------------------------------------------------------
    [Header("Rocket Velocity Upgrades")]
    public float defaultVelocity = 100;
    private float velocity;
    public float Velocity
    {
        get => velocity;
        set => velocity = value;
    }
    [SerializeField] private int velocityUpgradeStep = 0;
    public float[] velocityUpgradeValues = { 125, 200, 300 };
    public int[] velocityUpgradeCosts = { 5000, 20000, 40000 };
    // *****************************************************************************************************************
    
    
    // RELOAD SPEED ----------------------------------------------------------------------------------------------------
    [Header("Reload Speed Upgrades")]
    public float defaultReloadSpeed = 2.5f;
    private float reloadSpeed;
    public float ReloadSpeed
    {
        get => reloadSpeed;
        set => reloadSpeed = value;
    }
    [SerializeField] private int reloadSpeedUpgradeStep = 0;
    public float[] reloadSpeedUpgradeValues = { 2.25f, 2.0f, 1.5f };
    public int[] reloadSpeedUpgradeCosts = { 10000, 15000, 20000 };
    // *****************************************************************************************************************

    // Number of Rockets -----------------------------------------------------------------------------------------------
    [Header("Number of Rockets")] 
    public int defaultNumberOfShots = 2;
    private int numberOfShots;
    public int NumberOfShots
    {
        get => numberOfShots;
        private set => numberOfShots = value;
    }

    public bool twoRocketUpgradePurchased = false;

    public int twoRocketUpgradeCost = 15000;
    // *****************************************************************************************************************

    [Header("Cards")] 
    public UpgradeCard damageCard;
    public UpgradeCard radiusCard;
    public UpgradeCard velocityCard;
    public UpgradeCard reloadCard;
    
    public void ResetObject()
    {
        Damage = defaultDamage;
        damageUpgradeStep = 0;
        damageCard.maxUpgradeReached = false;
        damageCard.upgradeCost = damageUpgradeCosts[0];
        damageCard.getCurrentValue = null;
        damageCard.getUpgradeValue = null;
        damageCard.updateCard = null;

        ExplosionRadius = defaultExplosionRadius;
        radiusUpgradeStep = 0;

        Velocity = defaultVelocity;
        velocityUpgradeStep = 0;

        ReloadSpeed = defaultReloadSpeed;
        reloadSpeedUpgradeStep = 0;

        NumberOfShots = defaultNumberOfShots;
        twoRocketUpgradePurchased = false;

        UpgradeProjectile();
        
        initialized = false;
    }

    public void Init()
    {
        if (initialized) return;
        
        Damage = defaultDamage;
        Velocity = defaultVelocity;
        ExplosionRadius = defaultExplosionRadius;
        ReloadSpeed = defaultReloadSpeed;
        NumberOfShots = defaultNumberOfShots;

        damageCard.upgradeCost = damageUpgradeCosts[0];
        damageCard.getUpgradeValue = GetNextDamageUpgrade;
        damageCard.getCurrentValue = () => { return Damage; };
        damageCard.purchase = UpgradeDamage;
        damageCard.updateCard = UpdateDamageCard;
        
        radiusCard.upgradeCost = radiusUpgradeCosts[0];
        radiusCard.getUpgradeValue = GetNextExplosionRadiusUpgrade;
        radiusCard.getCurrentValue = () => { return ExplosionRadius; };
        radiusCard.purchase = UpgradeRadius;
        radiusCard.updateCard = UpdateRadiusCard;
        
        velocityCard.upgradeCost = velocityUpgradeCosts[0];
        velocityCard.getUpgradeValue = GetNextVelocityUpgrade;
        velocityCard.getCurrentValue = () => { return Velocity; };
        velocityCard.purchase = UpgradeVelocity;
        velocityCard.updateCard = UpdateVelocityCard;
        
        reloadCard.upgradeCost = reloadSpeedUpgradeCosts[0];
        reloadCard.getUpgradeValue = GetNextReloadSpeedUpgrade;
        reloadCard.getCurrentValue = () => { return ReloadSpeed; };
        reloadCard.purchase = UpgradeReloadSpeed;
        reloadCard.updateCard = UpdateReloadSpeedCard;
        
        initialized = true;
    }

    public void UpgradeDamage()
    {
        if (damageUpgradeStep > damageUpgradeCosts.Length - 1) return;
        if (gameData.gold >= damageUpgradeCosts[damageUpgradeStep])
        {
            Damage = damageUpgradeValues[damageUpgradeStep];
            
            UpgradeProjectile();
            
            int cost = damageUpgradeCosts[damageUpgradeStep];
            gameData.gold -= cost;
            ++damageUpgradeStep;
            // return cost;
        }
        
        // return 0;
    }
    
    public void UpgradeVelocity()
    {
        if (velocityUpgradeStep > velocityUpgradeCosts.Length - 1) return;
        
        if (gameData.gold >= velocityUpgradeCosts[velocityUpgradeStep])
        {
            Velocity = velocityUpgradeValues[velocityUpgradeStep];
            int cost = velocityUpgradeCosts[velocityUpgradeStep];
            
            gameData.gold -= cost;
            ++velocityUpgradeStep;
        }
        
    }
    
    public void UpgradeRadius()
    {
        if (radiusUpgradeStep > radiusUpgradeCosts.Length - 1) return;
        
        if (gameData.gold >= radiusUpgradeCosts[radiusUpgradeStep])
        {
            ExplosionRadius = radiusUpgradeValues[radiusUpgradeStep];

            int cost = radiusUpgradeCosts[radiusUpgradeStep];
            gameData.gold -= cost;
            
            ++radiusUpgradeStep;
        }
    }
    
    public void UpgradeReloadSpeed()
    {
        if (reloadSpeedUpgradeStep > reloadSpeedUpgradeCosts.Length - 1) return;
        
        if (gameData.gold >= reloadSpeedUpgradeCosts[reloadSpeedUpgradeStep])
        {
            ReloadSpeed = reloadSpeedUpgradeValues[reloadSpeedUpgradeStep];

            int cost = reloadSpeedUpgradeCosts[reloadSpeedUpgradeStep];
            gameData.gold -= cost;
            ++reloadSpeedUpgradeStep;
        }
    }
    
    // public int UpgradeTwoRockets()
    // {
    //     if (twoRocketUpgradePurchased) return 0;
    //     
    //     NumberOfShots = 2;
    //     twoRocketUpgradePurchased = true;
    //     return twoRocketUpgradeCost;
    // }

    private void UpgradeProjectile()
    {
        rocketAttributes.damage = Damage;
    }

    public float GetNextDamageUpgrade()
    {
        if (damageUpgradeStep >= damageUpgradeCosts.Length) return 0;
        else return damageUpgradeValues[damageUpgradeStep];
    }
    
    public float GetNextVelocityUpgrade()
    {
        if (velocityUpgradeStep >= velocityUpgradeCosts.Length) return 0;
        else return velocityUpgradeValues[velocityUpgradeStep];
    }
    
    public float GetNextExplosionRadiusUpgrade()
    {
        if (radiusUpgradeStep >= radiusUpgradeCosts.Length) return 0;
        else return radiusUpgradeValues[radiusUpgradeStep];
    }
    
    public float GetNextReloadSpeedUpgrade()
    {
        if (reloadSpeedUpgradeStep >= reloadSpeedUpgradeCosts.Length) return 0;
        else return reloadSpeedUpgradeValues[reloadSpeedUpgradeStep];
    }

    // public int GetTwoRocketUpgradeCost()
    // {
    //     return twoRocketUpgradeCost;
    // }
    
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
        if (velocityUpgradeStep >= velocityUpgradeValues.Length)
        {
            velocityCard.maxUpgradeReached = true;
            velocityCard.upgradeCost = 0;
            velocityCard.buttonInstance.gameObject.SetActive(false);
        }
        else
        {
            velocityCard.upgradeCost = velocityUpgradeCosts[velocityUpgradeStep];
        }
    }
    
    public void UpdateRadiusCard()
    {
        if (radiusUpgradeStep >= radiusUpgradeCosts.Length)
        {
            radiusCard.maxUpgradeReached = true;
            radiusCard.upgradeCost = 0;
            radiusCard.buttonInstance.gameObject.SetActive(false);
        }
        else
        {
            radiusCard.upgradeCost = radiusUpgradeCosts[radiusUpgradeStep];
        }
    }
    
    public void UpdateReloadSpeedCard()
    {
        if (reloadSpeedUpgradeStep >= reloadSpeedUpgradeCosts.Length)
        {
            radiusCard.maxUpgradeReached = true;
            radiusCard.upgradeCost = 0;
            radiusCard.buttonInstance.gameObject.SetActive(false);
        }
        else
        {
            radiusCard.upgradeCost = reloadSpeedUpgradeCosts[reloadSpeedUpgradeStep];
        }
    }
}
