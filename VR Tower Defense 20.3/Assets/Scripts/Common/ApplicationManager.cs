using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    public static ApplicationManager Instance;
    public GameData gameData;
    public Devices devices;
    public MachineGunUpgrades machineGunUpgrades;
    public RocketLauncherUpgrades rocketLauncherUpgrades;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        gameData.ResetObject();
        devices.ResetObject();
        machineGunUpgrades.ResetObject();
        rocketLauncherUpgrades.ResetObject();
    }
}
