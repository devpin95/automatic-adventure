using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerRotateController : MonoBehaviour
{
    [SerializeField] private Devices _devices;
    [Tooltip("Overwritten by the calling object")]
    public float rotateSpeed;
    public GameObject towerTrans;
    public GameObject playerRig;
    public Transform towerAnchorPoint;
    public InputActionReference rotateAction;
    private float lastAngle = 0;
    private bool firstRotationCall = true;
    
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
        Vector2 joystickVal = rotateAction.action.ReadValue<Vector2>();

        float angle = joystickVal.x * rotationSpeed * Time.deltaTime;

        if (Mathf.Abs(angle) <= Mathf.Abs(lastAngle))
        {
            angle = Mathf.Lerp(lastAngle, angle, Time.deltaTime * 5);
        }
        
        // if (Mathf.Abs(angle) >= Mathf.Abs(lastAngle))
        // {
        //     angle = Mathf.Lerp(angle, lastAngle, Time.deltaTime * 5);
        // }
        
        lastAngle = angle;

        towerTrans.transform.RotateAround(playerRig.transform.position, towerAnchorPoint.up, angle);
        transform.RotateAround(playerRig.transform.position, towerAnchorPoint.up, angle);
        playerRig.transform.RotateAround(playerRig.transform.position, playerRig.transform.up, angle);
    }
}
