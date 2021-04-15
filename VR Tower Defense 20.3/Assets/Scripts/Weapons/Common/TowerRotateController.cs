using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRotateController : MonoBehaviour
{
    [SerializeField] private Devices _devices;
    public float rotateSpeed;
    public GameObject towerTrans;
    public GameObject playerRig;
    public Transform towerAnchorPoint;
    
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
        if (_devices.RightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystickVal))
        {
            float angle = joystickVal.x * rotateSpeed * Time.deltaTime;
            towerTrans.transform.RotateAround(towerAnchorPoint.position, towerAnchorPoint.up, angle);
            transform.RotateAround(towerAnchorPoint.position, towerAnchorPoint.up, angle);
        }
    }
}
