using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class RocketLauncherController : MonoBehaviour
{
    public RocketLauncherUpgrades upgrades;

    public GameObject mainTower;
    private HeavyWeaponController _heavyWeaponController;
    
    [Header("Camera Properties")]
    public Camera cameraFeed;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin _noise;

    public float minZoom;
    public float maxZoom;

    public float cameraShakeMaxFrequency;
    public float cameraShakeMaxAmplitude;
    private float _cameraShakeFrequency = 0;
    private float _cameraShakeAmplitude = 0;
    // private float _cameraShakeFallRate = 0.001f;
    private float _cameraShakeDuration = 0.5f;
    private float _cameraShakeTime = 0.5f;

    [Header("Actions")] 
    public CEvent_Int switchActionMap;
    public InputActionReference aimAction;
    
    [Header("Interaction Variables")]
    public float shotDelay;
    public float majorRotationSpeed;
    public float minorRotationSpeed;
    public GameObject rotationCenter;
    public LayerMask shootableLayers;

    [Header("UI Variables")]
    public Color readyColor;
    public Color reloadingColor;
    private TextMeshProUGUI _rangeText;
    private TextMeshProUGUI[] _readyTexts;

    private float timeSinceLastShot;
    private Vector3 centerPosition;
    private bool selected = false;
    private bool triggerPulled = false;
    private bool _shotReady = true;
    private bool validHit = false;

    private bool[] _chambers;
    
    [SerializeField] private Devices _devices;
    private XRBaseInteractor _currentInteractor;
    [SerializeField] private GameObject tempHand;

    [SerializeField] private Transform _firingPoint;
    [SerializeField] private GameObject _rocketPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GetGUIObjects();
        
        _chambers = new bool[upgrades.NumberOfShots];

        for (int i = 0; i < _chambers.Length; ++i)
        {
            _chambers[i] = true;
        }
        
        SetReadyText();

        if (upgrades.NumberOfShots < _readyTexts.Length)
        {
            for (int i = upgrades.NumberOfShots; i < _readyTexts.Length; ++i)
            {
                _readyTexts[i].text = "N/A";
                _readyTexts[i].color = reloadingColor;
            }
        }
        
        _noise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _noise.m_AmplitudeGain = 1;
        _noise.m_FrequencyGain = 1;
        
        cinemachineVirtualCamera.m_Lens.FieldOfView = minZoom;
    }

    // Update is called once per frame
    void Update()
    {
        centerPosition = transform.position;

        CastTarget();
        MoveCamera();
        ShakeCamera();
    }
    
    private void SetReadyText()
    {
        for (int i = 0; i < _chambers.Length; ++i)
        {
            if (_chambers[i])
            {
                _readyTexts[i].color = readyColor;
                _readyTexts[i].text = "READY";
            }
            else
            {
                _readyTexts[i].color = reloadingColor;
                _readyTexts[i].text = "LOADING";
            }
        }
    }

    public void Fire()
    {
        if (_shotReady && validHit)
        {
            triggerPulled = false;

            int readyChamber = CheckChambers();

            if (readyChamber >= 0)
            {
                GameObject rocket = Instantiate(_rocketPrefab, _firingPoint.position, _firingPoint.transform.rotation);
                rocket.transform.RotateAround(rocket.transform.position, rocket.transform.right, 90);

                _chambers[readyChamber] = false;
                StartCoroutine(ReloadShot(readyChamber));
                SetReadyText();
            }

            _cameraShakeAmplitude = cameraShakeMaxAmplitude;
            _cameraShakeFrequency = cameraShakeMaxFrequency;
            _cameraShakeTime = 0.0f;
        }
    }

    private int CheckChambers()
    {
       
        for (int i = 0; i < _chambers.Length; ++i)
        {
            if (_chambers[i])
            {
                return i;
            }
        }

        return -1;
    }
    
    IEnumerator ReloadShot(int chamber)
    {
        yield return new WaitForSeconds(upgrades.ReloadSpeed);
        _chambers[chamber] = true;
        SetReadyText();
    }

    public void CameraFeedZoomChange(float zoom)
    {
        Debug.Log("Rocket Launcher Zoom " + zoom);
        
        float clampedZoom = Mathf.Lerp(maxZoom, minZoom, zoom);
        cinemachineVirtualCamera.m_Lens.FieldOfView = clampedZoom;
        Debug.Log(clampedZoom);
    }

    private void CameraFeedLookAtPoint()
    {
        RaycastHit hit;
        validHit = false;
        if (Physics.Raycast(_firingPoint.position, _firingPoint.forward, out hit, Mathf.Infinity, shootableLayers))
        {  
            cameraFeed.transform.LookAt(hit.point);
        }
        else
        {
            cameraFeed.transform.rotation = Quaternion.LookRotation(_firingPoint.forward, _firingPoint.up);
        }
    }

    private void GetGUIObjects()
    {
        HeavyWeaponGuiSearcher guiSearcher = GetComponent<HeavyWeapon>().gui.GetComponent<HeavyWeaponGuiSearcher>();
        _rangeText = guiSearcher.FindSingleGUIElementByName("Distance").GetComponent<TextMeshProUGUI>();

        List<GameObject> rstates = guiSearcher.FindGUIElementsByName("R State");
        _readyTexts = new TextMeshProUGUI[rstates.Count];

        for (int i = 0; i < rstates.Count; ++i)
        {
            _readyTexts[i] = rstates[i].GetComponent<TextMeshProUGUI>();
            _readyTexts[i].SetText("PPPUPU " + i);
        }
    }

    private void MoveCamera()
    {
        Vector2 joystickVal = aimAction.action.ReadValue<Vector2>();
        float angle = joystickVal.x * majorRotationSpeed * Time.deltaTime;
        transform.RotateAround(rotationCenter.transform.position, transform.up, angle);
                
        angle = -joystickVal.y * minorRotationSpeed * Time.deltaTime;
        transform.RotateAround(rotationCenter.transform.position, transform.right, angle);

        // cancel out any rotation on the z, there's probably a better way to do this
        transform.Rotate(0, 0, -transform.eulerAngles.z);
    }
    
    public void WeaponSelected(bool selected)
    {
        if (selected)
        {
            // the weapon is now being controlled
            switchActionMap.Raise((int)PlayerActionStateManager.ActionMap.HeavyWeaponRocketLauncher);
        }
        else
        {
            // the weapon is no longer being controlled
            switchActionMap.Raise((int)PlayerActionStateManager.ActionMap.Freemovement);
        }
    }

    private void CastTarget()
    {
        RaycastHit hit;
        validHit = false;
        if (Physics.Raycast(_firingPoint.position, _firingPoint.forward, out hit, Mathf.Infinity, shootableLayers))
        {
            Vector3 impactPoint = hit.point;
            _rangeText.text = Vector3.Distance(_firingPoint.position, impactPoint).ToString("F2") + "m";
            validHit = true;
        }
        else _rangeText.text = "--";
    }

    private void ShakeCamera()
    {
        _cameraShakeTime += Time.deltaTime;
        _noise.m_AmplitudeGain = Mathf.Lerp(cameraShakeMaxAmplitude, 0, _cameraShakeTime / _cameraShakeDuration);
        _noise.m_FrequencyGain = Mathf.Lerp(cameraShakeMaxFrequency, 0, _cameraShakeTime / _cameraShakeDuration);
    }
}
