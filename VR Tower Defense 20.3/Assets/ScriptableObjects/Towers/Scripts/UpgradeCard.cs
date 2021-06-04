using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Upgrades/UI/Card")]
public class UpgradeCard : ScriptableObject
{
    [NonSerialized] public GameObject buttonInstance;
    
    [Header("UI Display and control variables")]
    public string upgradeName;
    public string upgradeDescription;
    public int upgradeCost;
    public bool maxUpgradeReached = false;
    public bool createIfMaxUpgradeReachedOnStartup;
    
    [Header("Attribute Indicator")]
    [Tooltip("Possible values should be set in the UI controller responsible for handling the UI response.")]
    public string upgradeType;
    
    public Func<float> getUpgradeValue; // function that returns the new attribute value of the upgrade
    public UnityAction updateCard; // action that will update the card to get any new changes
    public UnityAction purchase; // action that will make the appropriate changes when an upgrade is purchased
}
