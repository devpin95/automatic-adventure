using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 0.5f;
    public float pitchSpeed = 0.25f;
    public Transform tower;

    [Header("Actions")]
    public InputActionReference freemove;
    [FormerlySerializedAs("pitch")] public InputActionReference pitchUp;
    public InputActionReference pitchDown;
    
    private Transform _playerCamera;
    private Transform _cameraOffset;

    private bool pitchingUp = false;
    private bool pitchingDown = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.Find("Camera Offset");
        _playerCamera = _cameraOffset.transform.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer( freemove.action.ReadValue<Vector2>() );

        if ( pitchUp.action.ReadValue<float>() > 0.5f ) 
        {
            Vector3 pos = _cameraOffset.position;
            pos.y = _cameraOffset.position.y + pitchSpeed * Time.deltaTime;
            _cameraOffset.position = pos;
        }
        else if (pitchDown.action.ReadValue<float>() > 0.5f)
        {
            Vector3 pos = _cameraOffset.position;
            pos.y = _cameraOffset.position.y - pitchSpeed * Time.deltaTime;
            _cameraOffset.position = pos;
        }
    }

    private void MovePlayer(Vector3 input)
    {
        Vector3 translation = new Vector3(input.x, 0, input.y);
        
        translation = Quaternion.AngleAxis(_playerCamera.transform.eulerAngles.y, Vector3.up) * translation;
    
        transform.Translate(translation * Time.deltaTime * movementSpeed, Space.World);
    
        Vector3 pos = transform.position;
        pos.y = tower.gameObject.transform.Find("Tower Anchor Point").position.y;
        transform.position = pos;
    }
}
