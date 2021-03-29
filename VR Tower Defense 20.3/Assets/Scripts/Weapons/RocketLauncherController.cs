using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RocketLauncherController : MonoBehaviour
{
    public float shotDelay;
    public float majorRotationSpeed;
    public float minorRotationSpeed;
    public GameObject rotationCenter;
    private float timeSinceLastShot;
    private Vector3 centerPosition;
    private bool selected = false;
    private bool firing = false;
    [SerializeField] private Devices _devices;
    private XRBaseInteractor _currentInteractor;
    [SerializeField] private GameObject tempHand;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        centerPosition = transform.position;
        if (selected && _devices.isReady)
        {
            Vector2 joystickVal;
            if (_devices.rightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystickVal))
            {
                float angle = joystickVal.x * majorRotationSpeed * Time.deltaTime;
                transform.RotateAround(rotationCenter.transform.position, transform.up, angle);
                
                angle = -joystickVal.y * minorRotationSpeed * Time.deltaTime;
                transform.RotateAround(rotationCenter.transform.position, transform.forward, angle);

                transform.Rotate(-transform.eulerAngles.x, 0, 0);
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
}
