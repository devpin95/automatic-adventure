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
    public LayerMask _targetLayers;
    public float wallDamage;
    public int numberOfHitsBeforeDestroy;

    public EnemyKilledEvent killed;
    public EnemyAttributes attributes;

    private int hitCount = 0;
    private Vector3 _targetDir;
    private Vector3 _targetPoint;
    private Rigidbody _rb;
    private Vector3 _spawnPos;
    private bool isLaunched = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnPos = transform.position;
        _rb = GetComponent<Rigidbody>();
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.forward, out hit, Mathf.Infinity, _targetLayers))
        {
            _targetPoint = hit.point;
        }

        _targetDir = _targetPoint - transform.position;
        _targetDir = _targetDir.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z);
        // _targetDir = targetPos - transform.position;
        if (_rb.velocity.magnitude < maxVelocity)
        {
            _rb.AddForce(_targetDir * movementSpeed, ForceMode.Acceleration);
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
            killed.Raise(attributes.EnemyValue);
            Destroy(gameObject);
        } 
        else if (other.gameObject.CompareTag("Ball Launcher"))
        {
            maxVelocity *= 2;
            movementSpeed *= 2;
            isLaunched = true;
            _rb.AddForce(_targetDir.normalized * movementSpeed * 2, ForceMode.Impulse);
        } 
        else if (other.gameObject.CompareTag("Wall"))
        {
            WallManager wm = other.transform.GetComponent<WallManager>();
            if (!wm) return;
            
            wm.HitWall(wallDamage);
            
            ++hitCount;
            if (hitCount >= numberOfHitsBeforeDestroy)
            {
                Destroy(gameObject);
            }
            
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            _rb.AddForce(-_targetDir, ForceMode.Impulse);
        }
    }

    public void TakeIndirectHit(float damage)
    {
        killed.Raise(attributes.EnemyValue);   
    }
}
