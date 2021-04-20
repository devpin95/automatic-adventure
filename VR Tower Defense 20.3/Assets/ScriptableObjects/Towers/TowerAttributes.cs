using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

[CreateAssetMenu(menuName = "ScriptableObjects/Towers")]
public class TowerAttributes : ScriptableObject
{
    public string towerName;
    public int towerCost;
    public GameObject prefab;
    public bool purchased = false;
}
