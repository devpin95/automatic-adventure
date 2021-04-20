using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBulletPool : ObjectPool
{
    public static MachineGunBulletPool SharedInstance;

    private void Awake()
    {
        SharedInstance = this;
    }
}
