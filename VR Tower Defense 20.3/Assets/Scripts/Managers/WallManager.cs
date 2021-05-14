using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public GameData _gameData;
    public TextMeshProUGUI mainWallHealthText;
    public Gradient healthTextGradient;
    
    public TextMeshProUGUI wallMoneyValue;
    public TextMeshProUGUI wallWaveValue;

    [SerializeField] private float startingHealth;
    private float currentHealth;

    private float currentMoney = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        HitWall(0f);
        UpdateWave(_gameData.Wave);
        UpdateMoney(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitWall(float damage)
    {
        int healthInt = (int) (_gameData.wallCurrentHealth);
        mainWallHealthText.text = "Integrity " + healthInt;
        // mainWallHealthText.color = healthTextGradient.Evaluate((_gameData.wallCurrentHealth / _gameData.wallMaxHealth));
    }

    public void UpdateMoney(int value, bool s = false)
    {
        wallMoneyValue.text = "Credit " + _gameData.gold;
    }

    public void UpdateWave(int wave)
    {
        wallWaveValue.text = "Wave " + (_gameData.Wave + 1);
    }
}
