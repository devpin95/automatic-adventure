using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactPool : ObjectPool
{
    public static BulletImpactPool SharedInstance;
    private void Awake()
    {
        SharedInstance = this;
    }
}
