using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Upgrades/UI/Card")]
public class UpgradeCard : ScriptableObject
{
    public GameObject buttonInstance;
    public string upgradeName;
    public string upgradeDescription;
    public int upgradeCost;
    public bool maxUpgradeReached = false;
    public string maxUpgradeNotification;

    public UnityEvent<UpgradeCard> purchased;
}
