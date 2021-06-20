using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusesUIController : MonoBehaviour
{
    public Bonuses bonuses;
    public GameData gameData;
    
    [Header("Tracked Values")]
    public TextMeshProUGUI timeTMP;
    public TextMeshProUGUI enemiesTMP;
    public TextMeshProUGUI damageTMP;
    
    [Header("Total")]
    public TextMeshProUGUI totalTMP;
    
    [Header("Bonuses")]
    public TextMeshProUGUI timeBonusTMP;
    public TextMeshProUGUI killBonusTMP;
    public TextMeshProUGUI damageBonusTMP;

    // Start is called before the first frame update
    void Start()
    {
        int totalBonuses = bonuses.CalculateBonuses();
        gameData.gold += totalBonuses;
        
        totalTMP.text = "Total: C. " + totalBonuses.ToString("n0");
        
        timeTMP.text = bonuses.TimeToString();
        timeBonusTMP.text = "+C. " + bonuses.timeBonus.ToString("n0");
        
        enemiesTMP.text = bonuses.enemiesKilled.ToString("n0");
        killBonusTMP.text = "+C. " + bonuses.killBonus.ToString("n0");
        
        damageTMP.text = bonuses.wallDamageTaken.ToString("n2");
        damageBonusTMP.text = "+C. " + bonuses.damageBonus.ToString("n0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
