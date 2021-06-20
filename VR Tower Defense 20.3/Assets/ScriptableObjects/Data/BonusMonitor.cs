using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusMonitor : MonoBehaviour
{
    [SerializeField] private Bonuses _bonusMonitor;
    [SerializeField] private GameData _game;

    private float _wallStartingHealth;
    private float _startTime;

    private void Start()
    {
        _bonusMonitor.ResetBonuses();
        _bonusMonitor.stintModifier = (_game.Wave / _game.stintLength) + 1;
        _bonusMonitor.wallMaxHealth = _game.wallMaxHealth;
        _wallStartingHealth = _game.wallCurrentHealth;
        _startTime = Time.time;
    }

    public void EnemyKilledHandler(int val, bool s)
    {
        Debug.Log("BONUS ENEMY KILLEd");
        if ( val > 0 ) _bonusMonitor.enemiesKilled++;
    }

    public void StintEndedHandler()
    {
        Debug.Log("BONUS STINT ENDED");
        _bonusMonitor.wallDamageTaken = _wallStartingHealth - _game.wallCurrentHealth;
        _bonusMonitor.stintTime = Time.time - _startTime;
    }
}
