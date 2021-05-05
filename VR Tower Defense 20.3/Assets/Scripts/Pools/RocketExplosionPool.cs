using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosionPool : ObjectPool
{
    public static RocketExplosionPool Instance;

    private void Awake()
    {
        Instance = this;
    }
}
