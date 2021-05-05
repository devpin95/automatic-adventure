using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncherPool : ObjectPool
{
    public static BallLauncherPool Instance;
    public EnemyAttributes attributes;
    
    private void Awake()
    {
        Instance = this;
        attributes.pool = this;
    }
}
