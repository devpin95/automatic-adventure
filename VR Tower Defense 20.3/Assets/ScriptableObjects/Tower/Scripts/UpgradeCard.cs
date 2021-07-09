using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Upgrades/UI/Card")]
public class UpgradeCard : ScriptableObject
{
    public enum FormatTypes
    {
        None,
        N0,
        N2
    }

    public static Dictionary<FormatTypes, string> FormatTypeStrings = new Dictionary<FormatTypes, string>
    {
        {FormatTypes.None, ""},
        {FormatTypes.N0, "n0"},
        {FormatTypes.N2, "n2"}
    };
    
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
    public Func<float> getCurrentValue; // function that returns the current attribute value
    public UnityAction updateCard; // action that will update the card to get any new changes
    public UnityAction purchase; // action that will make the appropriate changes when an upgrade is purchased
}
