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
    public WallUpgrades wallUpgrades;
    public BountyList bountyList;
    
    
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
        
        machineGunUpgrades.Init();
        rocketLauncherUpgrades.Init();
        wallUpgrades.Init();
    }

    private void OnApplicationQuit()
    {
        gameData.ResetObject();
        devices.ResetObject();
        machineGunUpgrades.ResetObject();
        rocketLauncherUpgrades.ResetObject();
        wallUpgrades.ResetObject();
        bountyList.ResetObject();
    }
}
