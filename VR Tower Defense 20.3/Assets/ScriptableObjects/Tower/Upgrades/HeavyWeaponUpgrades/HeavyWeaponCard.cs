using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heavy Weapons/Card")]
public class HeavyWeaponCard : ScriptableObject
{
    public enum HeavyWeaponType
    {
        Explosive,
        Range,
        Laser
    }

    public static Dictionary<HeavyWeaponType, string> HeavyWeaponTypeNames = new Dictionary<HeavyWeaponType, string>()
    {
        {HeavyWeaponType.Explosive, "Explosive"},
        {HeavyWeaponType.Range, "Range"},
        {HeavyWeaponType.Laser, "Laser"}
    };
    
    public string weaponName;
    public string description;
    public float baseDamage;
    public float currentDamage;
    public float rpm;
    public bool isUpgradeable;
    public HeavyWeaponType heavyWeaponType;
    public int cost;
    public bool purchased = false;

    [Space(11)] 
    public GameObject prefab;
    public float cameraOrthographicSize;
    public Sprite itemPreview;
    public Vector3 previewOffset;
}
