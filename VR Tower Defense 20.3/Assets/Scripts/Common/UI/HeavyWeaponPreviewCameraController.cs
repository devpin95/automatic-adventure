using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeaponPreviewCameraController : MonoBehaviour
{
    public Transform lookAt;
    private float _startingX;
    private float _startingZ;

    private float _maxXDis = 5;
    private float _maxZDis = 5;

    private float _accumulator = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        _startingX = pos.x;
        _startingZ = pos.z;
    }

    // Update is called once per frame
    void Update()
    {
        _accumulator += Time.deltaTime * 0.2f;
        Vector3 newPos = transform.position;
        newPos.x = _startingX + Mathf.Sin(_accumulator) * _maxXDis;
        newPos.z = _startingZ + Mathf.Cos(_accumulator) * _maxZDis;

        transform.position = newPos;
        
        transform.LookAt(lookAt);
    }
}
