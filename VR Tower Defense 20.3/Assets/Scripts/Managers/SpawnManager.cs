using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Area Attributes")]
    public Vector3 spawnCenter;
    public float spawnAreaMajorWidth;
    public float spawnAreaMinorWidth;

    [Header("Enemy Attributes")]
    public GameObject enemyType;
    public Transform targetTrans;
    public int numEnemies;

    private float _pollTime = 5.0f;
    private float _pollTimer = 0.0f;

    private bool _playerDeviceReady = false;
    // Start is called before the first frame update
    void Start()
    {
        InputDevices.deviceConnected += OnDeviceConnected;
        InputDevices.deviceDisconnected += OnDeviceDisconnected;
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        _pollTimer += Time.deltaTime;

        if (_pollTimer > _pollTime && _playerDeviceReady)
        {
            _pollTimer = 0;
                
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                SpawnEnemies();
            }
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < numEnemies; ++i)
        {
            float randXpos = UnityEngine.Random.Range(spawnCenter.x - spawnAreaMajorWidth, spawnCenter.x + spawnAreaMajorWidth);
            float randZpos = UnityEngine.Random.Range(spawnCenter.z - spawnAreaMajorWidth, spawnCenter.z + spawnAreaMajorWidth);
            Vector3 spawnPos = new Vector3(randXpos, 1, randZpos);
            GameObject enemy = Instantiate(enemyType, spawnPos, UnityEngine.Random.rotation);
            enemy.GetComponent<BasicEnemyController>().target = targetTrans;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(spawnCenter, new Vector3(spawnAreaMajorWidth * 2, 1, spawnAreaMinorWidth * 2));
    }

    public void OnDeviceConnected(InputDevice d)
    {
        _playerDeviceReady = true;
    }

    public void OnDeviceDisconnected(InputDevice d)
    {
        _playerDeviceReady = false;
    }
}
