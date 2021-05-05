using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBallPool : ObjectPool
{
    public static CanonBallPool Instance;
    public EnemyAttributes attributes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            attributes.pool = Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
