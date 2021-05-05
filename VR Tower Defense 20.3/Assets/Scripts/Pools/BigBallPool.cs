using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBallPool : ObjectPool
{
    public static BigBallPool Instance;
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
