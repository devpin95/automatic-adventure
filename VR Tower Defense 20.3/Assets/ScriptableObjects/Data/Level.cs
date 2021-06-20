using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Level
{
    [FormerlySerializedAs("waveGroup")] public WaveGroup[] waveGroups;
}
