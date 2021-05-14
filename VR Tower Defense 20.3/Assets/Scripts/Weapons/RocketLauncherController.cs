using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class RocketLauncherController : MonoBehaviour
{
    public RocketLauncherUpgrades upgrades;

    public GameObject mainTower;
    private HeavyWeaponController _heavyWeaponController;
    public Camera cameraFeed;

    public float minZoom;
    public float maxZoom;
    
    [Header("Interaction Variables")]
    public float shotDelay;
    public float majorRotationSpeed;
    public float minorRotationSpeed;
    public GameObject rotationCenter;
    public LayerMask shootableLayers;

    [Header("UI Variables")]
    public Color readyColor;
    public Color reloadingColor;
    public TextMeshProUGUI rangeText;
    // private TextMeshPro tmp_rangeText;
    public TextMeshProUGUI[] readyTexts;
    // private TextMeshPro tmp_readyText;
    
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
        _heavyWeaponController = mainTower.transform.Find("Heavy Weapon Terminal").GetComponent<HeavyWeaponController>();
        
        _chambers = new bool[upgrades.NumberOfShots];

        for (int i = 0; i < _chambers.Length; ++i)
        {
            _chambers[i] = true;
        }
        
        // SetReadyText();

        if (upgrades.NumberOfShots < readyTexts.Length)
        {
            for (int i = upgrades.NumberOfShots - 1; i < readyTexts.Length; ++i)
            {
                readyTexts[i].text = "N/A";
                readyTexts[i].color = reloadingColor;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        centerPosition = transform.position;
        if (selected && _devices.IsReady)
        {
            RaycastHit hit;
            validHit = false;
            if (Physics.Raycast(_firingPoint.position, _firingPoint.forward, out hit, Mathf.Infinity, shootableLayers))
            {
                Vector3 impactPoint = hit.point;
                // rangeText.text = Vector3.Distance(_firingPoint.position, impactPoint).ToString("F2") + "m";
                validHit = true;
            }
            else
            {
                // rangeText.text = "--";
            }
            
            Vector2 joystickVal;
            if (_devices.RightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystickVal))
            {
                float angle = joystickVal.x * majorRotationSpeed * Time.deltaTime;
                transform.RotateAround(rotationCenter.transform.position, transform.up, angle);
                
                angle = -joystickVal.y * minorRotationSpeed * Time.deltaTime;
                transform.RotateAround(rotationCenter.transform.position, transform.right, angle);

                // cancel out any rotation on the x, there's probably a better way to do this
                transform.Rotate(0, 0, -transform.eulerAngles.z);
            }
        }
    }

    public void OnSelectEvent(bool s)
    {
        selected = s;
    }

    private void SetReadyText()
    {
        for (int i = 0; i < _chambers.Length; ++i)
        {
            if (_chambers[i])
            {
                readyTexts[i].color = readyColor;
                readyTexts[i].text = "READY";
            }
            else
            {
                readyTexts[i].color = reloadingColor;
                readyTexts[i].text = "LOADING";
            }
        }
    }

    public void Fire()
    {
        if (_shotReady && validHit);
        triggerPulled = false;
        
        int readyChamber = CheckChambers();

        if (readyChamber >= 0)
        {
            GameObject rocket = Instantiate(_rocketPrefab, _firingPoint.position, _firingPoint.transform.rotation);
            rocket.transform.RotateAround(rocket.transform.position, rocket.transform.right, 90);

            _chambers[readyChamber] = false;
            StartCoroutine(ReloadShot(readyChamber));
            // SetReadyText();
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
        // SetReadyText();
    }

    public void CameraFeedZoomChange(float zoom)
    {
        Debug.Log("Rocket Launcher Zoom " + zoom);
        
        float clampedZoom = Mathf.Lerp(maxZoom, minZoom, zoom);
        cameraFeed.fieldOfView = clampedZoom;
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
            // rangeText.text = "--";
        }
    }
}
