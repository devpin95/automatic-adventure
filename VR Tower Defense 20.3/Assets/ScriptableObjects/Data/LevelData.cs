using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public Level[] levels;
    public CustomLightSettings[] stintLightSettings;
}
