using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    public Transform target;
    public float movementSpeed;
    public float maxVelocity;
    private Vector3 _targetDir;
    private Rigidbody _rb;
    private Vector3 _spawnPos;
    private bool isLaunched = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnPos = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z);
        _targetDir = targetPos - transform.position;
        if (_rb.velocity.magnitude < maxVelocity)
        {
            _rb.AddForce(_targetDir.normalized * movementSpeed, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && isLaunched)
        {
            maxVelocity /= 2;
            movementSpeed /= 2;
            isLaunched = false;
        }
        else if (other.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        } 
        else if (other.gameObject.CompareTag("Ball Launcher"))
        {
            maxVelocity *= 2;
            movementSpeed *= 2;
            isLaunched = true;
            _rb.AddForce(_targetDir.normalized * movementSpeed * 2, ForceMode.Impulse);
        }
    }
}
