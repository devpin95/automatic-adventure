using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public GameObject pooledObject;
    private List<GameObject> _pool = new List<GameObject>();
    public int _poolCapacity;

    // Start is called before the first frame update
    void Start()
    {
        GameObject temp;

        for (int i = 0; i < _poolCapacity; ++i)
        {
            temp = Instantiate(pooledObject, Vector3.zero, pooledObject.transform.rotation);
            temp.SetActive(false);
            _pool.Add(temp);
        }
    }

    public GameObject GetPooledObject()
    {
        // is there a better way of keeping track of which object is ready to be taken out of the pool?
        for (int i = 0; i < _poolCapacity; ++i)
        {
            if (!_pool[i].activeInHierarchy)
            {
                return _pool[i];
            }
        }

        return null;
    }

    public ObjectPool GetInstance()
    {
        return ObjectPool.SharedInstance;
    }
}
