using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bonus Data")]
public class Bonuses : ScriptableObject
{
    // public class Bonus
    // {
    //     public string name;
    //     public int bonus;
    // }
    
    public float stintTime = 0;
    public int enemiesKilled = 0;
    public float wallDamageTaken = 0;
    public float wallMaxHealth = 0;
    public int stintModifier;
    // public List<Bonus> bonuses = new List<Bonus>();

    public int timeBonus;
    public int damageBonus;
    public int killBonus;

    public void ResetBonuses()
    {
        stintTime = 0;
        enemiesKilled = 0;
        wallDamageTaken = 0;
        // bonuses = new List<Bonus>();
    }

    public int CalculateBonuses()
    {
        // bonuses = new List<Bonus>();
        
        int bonus = 0;

        timeBonus = CalculateStintTimeBonus();
        bonus += timeBonus;
        // Bonus timeBonusData = new Bonus();
        // timeBonusData.name = "Stint Time";
        // timeBonusData.bonus = timeBonus;
        // bonuses.Add(timeBonusData);
        
        damageBonus = CalculateDamageBonus();
        bonus += damageBonus;
        // Bonus damageBonusData = new Bonus();
        // damageBonusData.name = "Wall Damage";
        // damageBonusData.bonus = damageBonus;
        // bonuses.Add(damageBonusData);
        
        killBonus = CalculateEnemiesKilledBonus();
        bonus += killBonus;
        // Bonus enemiesKilledBonusData = new Bonus();
        // enemiesKilledBonusData.name = "Wall Damage";
        // enemiesKilledBonusData.bonus = killBonus;
        // bonuses.Add(enemiesKilledBonusData);

        return bonus;
    }

    private int CalculateStintTimeBonus()
    {
        int bonus = 500 * stintModifier;

        if (stintTime < (60 * 5 * stintModifier)) // 5 minutes * stint
        {
            bonus = 2500 * stintModifier;
        }
        else if (stintTime < (60 * 10 * stintModifier))
        {
            bonus = 1000 * stintModifier;
        }

        return bonus;
    }

    private int CalculateDamageBonus()
    {
        // float percModifier = wallDamageTaken / wallMaxHealth;
        int bonus = 500 * stintModifier;

        if (wallDamageTaken == 0)
        {
            bonus = 10000 * stintModifier;
        } 
        else if (wallDamageTaken < 10)
        {
            bonus = 5000 * stintModifier;
        } 
        else if (wallDamageTaken < 50)
        {
            bonus = 1000 * stintModifier;
        }

        return bonus;
    }

    private int CalculateEnemiesKilledBonus()
    {
        return enemiesKilled * 10;
    }

    public string TimeToString()
    {
        int minutes = (int)(stintTime / 60);
        string minStr = minutes.ToString();

        if (minutes < 10) minStr = "0" + minStr;
        
        float seconds = stintTime % 60;
        string secStr = seconds.ToString("n0");;

        if (seconds < 10) secStr = "0" + secStr;
        
        return minStr + ":" + secStr;
    }
}
