using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerTowerManager : MonoBehaviour
{
    public GameObject xrRig;
    private Transform _playerCamera;
    public float movementSpeed = 0.2f;

    private List<GameObject> _towers = new List<GameObject>();

    [SerializeField] private Devices _devices;
    [SerializeField] private GameData _gameData;
    private int _currentTower;
    private bool deviceReady = false;
    private bool previousPrimaryButtonDown = false;
    private bool previousSecondaryButtonDown = false;

    [SerializeField] private XRBaseInteractor _rh;
    [SerializeField] private XRBaseInteractor _lh;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] towerList = GameObject.FindGameObjectsWithTag("Tower");

        foreach (var tower in towerList)
        {
            _towers.Add(tower);
        }

        _currentTower = 0;
        Transform towerAnchor = _towers[0].gameObject.transform.Find("XRRig Anchor");
        xrRig.transform.position = towerAnchor.position;
        xrRig.transform.rotation = towerAnchor.rotation;

        _playerCamera = xrRig.transform.Find("Camera Offset").transform.Find("Main Camera");
        
        deviceReady = _devices.IsReady;
    }

    // Update is called once per frame
    void Update()
    {
        // if (!deviceReady) return;
        //
        // bool primaryButtonDown;
        // if (_devices.RightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out primaryButtonDown) && primaryButtonDown && primaryButtonDown != previousPrimaryButtonDown && !_devices.RightHandActive && !_devices.LeftHandActive)
        // {
        //     ChangeTowers();
        // }
        // previousPrimaryButtonDown = primaryButtonDown;
        //
        
        // if (!_devices.RightHandActive && !_devices.LeftHandActive)
        // {
        //     Vector2 joystickVal;
        //     if (_devices.RightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystickVal))
        //     {
        //         Vector3 translation = new Vector3(joystickVal.x, 0, joystickVal.y);
        //         translation = Quaternion.AngleAxis(_playerCamera.transform.eulerAngles.y, Vector3.up) * translation;
        //
        //         xrRig.transform.Translate(translation * Time.deltaTime * movementSpeed, Space.World);
        //
        //         Vector3 pos = xrRig.transform.position;
        //         pos.y = _towers[_currentTower].gameObject.transform.Find("Tower Anchor Point").position.y;
        //         xrRig.transform.position = pos;
        //     }
        // }
        
        //
        // bool secondaryButtonDown;
        // if (_devices.RightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out secondaryButtonDown) && secondaryButtonDown && secondaryButtonDown != previousSecondaryButtonDown)
        // {
        //     // reset the player's anchor point to their current position
        //     Transform towerAnchor = _towers[_currentTower].gameObject.transform.Find("XRRig Anchor");
        //     towerAnchor.position = new Vector3(_playerCamera.position.x, towerAnchor.position.y, _playerCamera.position.z);
        //     Vector3 playerRotation = new Vector3( 0, towerAnchor.eulerAngles.y, 0 );
        //     towerAnchor.transform.eulerAngles = playerRotation;
        //     // towerAnchor.rotation = Quaternion.Euler(playerRotation);
        // }
        //
        // previousSecondaryButtonDown = secondaryButtonDown;

    }

    public void OnDeviceConnectResponse()
    {
        deviceReady = true;
    }

    public void ChangeTowers(int tower = -1)
    {
        if (tower >= 0)
        {
            _currentTower = tower;
        }
        else
        {
            ++_currentTower;
            if (_currentTower >= _towers.Count)
            {
                _currentTower = 0;
            }
        }

        _gameData.currentActiveTower = _currentTower;
        
        Transform towerAnchor = _towers[_currentTower].gameObject.transform.Find("XRRig Anchor");
        xrRig.transform.position = towerAnchor.position;
        xrRig.transform.rotation = towerAnchor.rotation;
    }
    
}
