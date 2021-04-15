using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WallManager : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitWall(float damage)
    {
        currentHealth -= damage;
        var val = (int) currentHealth;
        mainWallHealthText.text = val.ToString();
        mainWallHealthText.color = healthTextGradient.Evaluate((currentHealth / startingHealth));
    }

    public void UpdateMoney(float value)
    {
        currentMoney += value;
        wallMoneyValue.text = currentMoney.ToString();
    }

    public void UpdateWave(int wave)
    {
        wallWaveValue.text = wave.ToString();
    }
}
