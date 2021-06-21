using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 0.5f;
    public Transform tower;
    
    [Header("Actions")]
    public InputActionReference freemove;
    public InputActionReference pitch;
    
    private Transform _playerCamera; 
    
    // Start is called before the first frame update
    void Start()
    {
        _playerCamera = transform.Find("Camera Offset").transform.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 joystickVal = freemove.action.ReadValue<Vector2>();
        Vector3 translation = new Vector3(joystickVal.x, 0, joystickVal.y);
        
        translation = Quaternion.AngleAxis(_playerCamera.transform.eulerAngles.y, Vector3.up) * translation;
    
        transform.Translate(translation * Time.deltaTime * movementSpeed, Space.World);
    
        Vector3 pos = transform.position;
        pos.y = tower.gameObject.transform.Find("Tower Anchor Point").position.y;
        transform.position = pos;
        
    }
}
