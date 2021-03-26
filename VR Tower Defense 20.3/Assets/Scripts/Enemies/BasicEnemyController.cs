using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    public Transform target;
    public float movementSpeed;
    private Vector3 _targetDir;
    private Rigidbody _rb;
    private Vector3 _spawnPos;
    
    // Start is called before the first frame update
    void Start()
    {
        _targetDir = Vector3.zero - transform.position;
        _spawnPos = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _targetDir = Vector3.zero - transform.position;
        _rb.AddForce(_targetDir.normalized * movementSpeed, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
