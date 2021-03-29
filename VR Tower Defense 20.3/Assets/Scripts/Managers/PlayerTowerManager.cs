using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class PlayerTowerManager : MonoBehaviour
{
    public GameObject xrRig;

    private List<GameObject> _towers = new List<GameObject>();

    [SerializeField] private Devices _devices;
    private int _currentTower;
    private bool deviceReady = false;
    private bool previousButtonDown = false;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] towerList = GameObject.FindGameObjectsWithTag("Tower");

        foreach (var tower in towerList)
        {
            _towers.Add(tower);
        }

        _currentTower = 0;
        Transform towerAnchor = _towers[0].gameObject.transform.Find("Tower Anchor Point");
        xrRig.transform.position = towerAnchor.position;
        xrRig.transform.rotation = towerAnchor.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!deviceReady) return;

        bool buttonDown;
        if (_devices.rightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out buttonDown) && buttonDown && buttonDown != previousButtonDown)
        {
            ChangeTowers();
        }
        
        previousButtonDown = buttonDown;

    }

    public void OnDeviceConnectResponse()
    {
        deviceReady = true;
    }

    private void ChangeTowers()
    {
        ++_currentTower;
        if (_currentTower >= _towers.Count)
        {
            _currentTower = 0;
        }
        
        Transform towerAnchor = _towers[_currentTower].gameObject.transform.Find("Tower Anchor Point");
        xrRig.transform.position = towerAnchor.position;
        xrRig.transform.rotation = towerAnchor.rotation;
    }

}
