using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    [SerializeField] private Devices _devices;
    [SerializeField] private GameData _gameData;
    [SerializeField] private LevelData _levelData;
    [SerializeField] private MachineGunUpgrades _machineGunUpgrades;
    [SerializeField] private RocketLauncherUpgrades _rocketLauncherUpgrades; 
    private Light mainDirectionLight;
    private GameObject _flareGuns;
    private GameObject _spotLights;

    private bool isPlayingScene = true;

    public CEvent FireFlare;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _machineGunUpgrades.Init();
        _rocketLauncherUpgrades.Init();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        InputDevices.deviceConnected += InitDevices;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_devices.IsReady)
        {
            Debug.Log("Device Not Ready");
        }
    }

    private void InitDevices(InputDevice inputDevice)
    {
        // _devicesReadyEvent.Raise();
        _devices.InitDevices(inputDevice);
        Debug.Log("GAME MANAGER: Init Devices");
    }

    public void HandleStintEndedCEvent()
    {
        LevelChanger.Instance.FadeToLevel(1);
    }

    public void WallHitHandler(float damage)
    {
        _gameData.wallCurrentHealth -= damage;
    }

    public void EnemyKilledHandler(int val, bool s = true)
    {
        _gameData.gold += val;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            Debug.Log("Loading the playing scene");
            mainDirectionLight = GameObject.FindWithTag("Sun").GetComponent<Light>();
            _flareGuns = GameObject.FindWithTag("Flare Guns");
            _spotLights = GameObject.FindWithTag("Spot Lights");
            Debug.Log(_flareGuns);
            Debug.Log(_spotLights);
            UpdateStintLightSettings(_gameData.Wave / 10);
            // FireFlare.Raise();
        }
    }

    public void UpdateStintLightSettings(int stint)
    {
        CustomLightSettings settings = _levelData.stintLightSettings[stint];

        // set skybox properties
        settings.skybox.SetFloat("_Exposure", settings.skyboxExposure);
        settings.skybox.SetFloat("_Rotation", settings.skyboxRotation);
        RenderSettings.skybox = settings.skybox;
        RenderSettings.ambientIntensity = settings.ambientLightIntensity;

        if (settings.fog)
        {
            RenderSettings.fogDensity = settings.fogDensity;
            RenderSettings.fogColor = settings.fogColor;
        }
        else
        {
            RenderSettings.fogDensity = 0;
            RenderSettings.fogColor = new Color(0, 0, 0, 0);
        }

        if (mainDirectionLight)
        {
            mainDirectionLight.intensity = settings.mainLightIntensity;
            mainDirectionLight.transform.eulerAngles = settings.mainLightRotation;
        }
        
        if (settings.isNight)
        {
            RenderSettings.ambientIntensity = 0;
            mainDirectionLight.intensity = 0;
            settings.skybox.SetFloat("_Exposure", 0);
            RenderSettings.reflectionIntensity = 0.05f;
        }
        else
        {
            if ( _flareGuns ) _flareGuns.SetActive(false);
            if ( _spotLights ) _spotLights.SetActive(false);
        }
    }
}
