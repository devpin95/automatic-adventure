using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class PoolManager : MonoBehaviour
{
    // [SerializeField] private List<string> poolNames = new List<string>();
    // [SerializeField] private List<Component> pools = new List<Component>();

    public string[] poolNames;
    public ObjectPool[] pools;

    private void Awake()
    {

    }

    public ObjectPool GetPoolInstanceByName(string name)
    {
        int index = -1;
        for (int i = 0; i < poolNames.Length; ++i)
        {
            if (poolNames[i].Equals(name))
            {
                index = i;
                break;
            }
        }

        if (index == -1) return null;

        return pools[index].GetInstance();
    }
}
 