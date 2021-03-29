using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRotateController : MonoBehaviour
{
    [SerializeField] private Devices _devices;
    public float rotateSpeed;
    public GameObject towerTrans;
    public GameObject playerRig;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateTower()
    {
        Vector2 joystickVal;
        if (_devices.rightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystickVal))
        {
            float angle = joystickVal.x * rotateSpeed * Time.deltaTime;
            towerTrans.transform.RotateAround(playerRig.transform.position, playerRig.transform.up, angle);
            transform.RotateAround(playerRig.transform.position, playerRig.transform.up, angle);
        }
    }
}
