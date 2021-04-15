using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Random = System.Random;

public class SpawnManager : MonoBehaviour
{
    public WaveStartedEvent waveStartedEvent;
    [Header("Spawn Area Attributes")]
    public Vector3 spawnCenter;
    public float spawnAreaMajorWidth;
    public float spawnAreaMinorWidth;

    public GameData gameData;
    private int numEnemies = 0;

    private float _pollTime = 5.0f;
    private float _pollTimer = 0.0f;

    private int wave = 1;

    private bool _playerDeviceReady = false;
    // Start is called before the first frame update
    void Start()
    {
        // SpawnEnemies();
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
        for ( int j = 0; j < gameData.easyEnemies.Length; ++j ) {
            var count = gameData.easyEnemies[j].WaveSpawnModifier * wave;

            if (gameData.easyEnemies[j].CountPer10WavesModifier && wave > 10)
            {
                count *= (wave / 10);
            }
            
            Debug.Log("Spawning " + count + " " + gameData.easyEnemies[j].enemyName);
            
            for (int i = 0; i < count; ++i)
            {
                Quaternion rotation = gameData.easyEnemies[j].Prefab.transform.rotation;
                Vector3 angles = gameData.easyEnemies[j].rotation;
                Quaternion modRotation = Quaternion.Euler( angles.x, angles.y, angles.z );

                if (gameData.easyEnemies[j].randomRotation)
                {
                    rotation = UnityEngine.Random.rotation;
                }
                
                Instantiate(gameData.easyEnemies[j].Prefab, RandomPosition(), rotation * modRotation);
            }
        }
        
        
        waveStartedEvent.Raise(wave);
        ++wave;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(spawnCenter, new Vector3(spawnAreaMajorWidth * 2, 1, spawnAreaMinorWidth * 2));
    }

    public void OnDeviceConnectedResponse()
    {
        _playerDeviceReady = true;
    }

    private Vector3 RandomPosition()
    {
        float randXpos = UnityEngine.Random.Range(spawnCenter.x - spawnAreaMajorWidth, spawnCenter.x + spawnAreaMajorWidth);
        float randZpos = UnityEngine.Random.Range(spawnCenter.z - spawnAreaMajorWidth, spawnCenter.z + spawnAreaMajorWidth);
        return new Vector3(randXpos, 1, randZpos);
    }
}
