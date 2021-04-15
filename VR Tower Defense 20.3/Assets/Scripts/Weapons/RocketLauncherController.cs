using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RocketLauncherController : MonoBehaviour
{
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
    public TextMeshProUGUI readyText;
    // private TextMeshPro tmp_readyText;
    
    private float timeSinceLastShot;
    private Vector3 centerPosition;
    private bool selected = false;
    private bool firing = false;
    private bool _shotReady = true;
    [SerializeField] private Devices _devices;
    private XRBaseInteractor _currentInteractor;
    [SerializeField] private GameObject tempHand;

    [SerializeField] private Transform _firingPoint;
    [SerializeField] private GameObject _rocketPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SetReadyText(true);
    }

    // Update is called once per frame
    void Update()
    {
        centerPosition = transform.position;
        if (selected && _devices.IsReady)
        {
            
            RaycastHit hit;
            bool validHit = false;
            if (Physics.Raycast(_firingPoint.position, _firingPoint.forward, out hit, Mathf.Infinity, shootableLayers))
            {
                Vector3 impactPoint = hit.point;
                rangeText.text = Vector3.Distance(_firingPoint.position, impactPoint).ToString("F2") + "m";
                validHit = true;
            }
            else
            {
                rangeText.text = "--";
            }
            
            Vector2 joystickVal;
            if (_devices.RightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystickVal))
            {
                float angle = joystickVal.x * majorRotationSpeed * Time.deltaTime;
                transform.RotateAround(rotationCenter.transform.position, transform.up, angle);
                
                angle = -joystickVal.y * minorRotationSpeed * Time.deltaTime;
                transform.RotateAround(rotationCenter.transform.position, transform.forward, angle);

                // cancel out any rotation on the x, there's probably a better way to do this
                transform.Rotate(-transform.eulerAngles.x, 0, 0);
            }

            if (firing)
            {
                if (_shotReady && validHit)
                {
                    GameObject rocket = Instantiate(_rocketPrefab, _firingPoint.position, _firingPoint.transform.rotation);
                    rocket.transform.RotateAround(rocket.transform.position, rocket.transform.right, 90);
                    _shotReady = false;
                    SetReadyText(false);
                    StartCoroutine(ReloadShot());
                }
            }
        }
    }
    
    public void OnSelectEnter(XRBaseInteractor interactor)
    {
        selected = true;
        tempHand.SetActive(true);
        _currentInteractor = interactor;
        print(_currentInteractor.tag);
    }

    public void OnSelectExit(XRBaseInteractor interactor)
    {
        selected = false;
        tempHand.SetActive(false);
        _currentInteractor = null;
    }
    
    public void OnActivate(XRBaseInteractor interactor)
    {
        firing = true;
    }
    
    public void OnDeactivate(XRBaseInteractor interactor)
    {
        firing = false;
    }

    IEnumerator ReloadShot()
    {
        yield return new WaitForSeconds(shotDelay);
        _shotReady = true;
        SetReadyText(true);
    }

    private void SetReadyText(bool state)
    {
        if (state)
        {
            readyText.color = readyColor;
            readyText.text = "READY";
        }
        else
        {
            readyText.color = reloadingColor;
            readyText.text = "LOADING";
        }
    }
}
