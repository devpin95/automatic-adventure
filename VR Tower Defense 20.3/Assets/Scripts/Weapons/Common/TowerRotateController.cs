using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRotateController : MonoBehaviour
{
    [SerializeField] private Devices _devices;
    [Tooltip("Overwritten by the calling object")]
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

    public void RotateTower(float rotationSpeed)
    {
        Vector2 joystickVal;
        if (_devices.RightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystickVal))
        {
            float angle = joystickVal.x * rotationSpeed * Time.deltaTime;
            towerTrans.transform.RotateAround(towerAnchorPoint.position, towerAnchorPoint.up, angle);
            transform.RotateAround(towerAnchorPoint.position, towerAnchorPoint.up, angle);
        }
    }
}
