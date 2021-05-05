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
    public int numberOfHitsBeforeDestroy;

    public CEvent_Int_Bool killed;
    public EnemyAttributes attributes;

    public CEvent_Float hitWall;

    private int hitCount = 0;
    private Vector3 _targetDir;
    private Vector3 _targetPoint;
    private Rigidbody _rb;
    private Vector3 _spawnPos;
    private bool isLaunched = false;

    private float health;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnPos = transform.position;
        _rb = GetComponent<Rigidbody>();

        movementSpeed += UnityEngine.Random.Range(-1, 1);

        GetTarget();

        health = attributes.startingHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
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
        else if (other.gameObject.CompareTag("Ball Launcher"))
        {
            maxVelocity *= 2;
            movementSpeed *= 2;
            isLaunched = true;
            _rb.AddForce(_targetDir.normalized * movementSpeed * 2, ForceMode.Impulse);
        } 
        else if (other.gameObject.CompareTag("Wall"))
        {
            hitWall.Raise(attributes.wallHitDamage);
            
            ++hitCount;
            if (hitCount >= numberOfHitsBeforeDestroy)
            {
                killed.Raise(0, attributes.countAsEnemy);
                DisablePooledObject();
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            _rb.AddForce(-_targetDir * 1.2f, ForceMode.Impulse);
        }
    }

    public void TakeIndirectHit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            killed.Raise(attributes.EnemyValue, attributes.countAsEnemy);
            DisablePooledObject();
        }
    }

    private void GetTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.forward, out hit, Mathf.Infinity, _targetLayers))
        {
            _targetPoint = hit.point;
        }

        _targetDir = _targetPoint - transform.position;
        _targetDir = _targetDir.normalized;
    }

    private void DisablePooledObject()
    {
        hitCount = 0;
        if (isLaunched)
        {
            isLaunched = false;
            movementSpeed /= 2;
            maxVelocity /= 2;
        }
        _rb.angularVelocity = Vector3.zero;
        _rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GetTarget();
        health = attributes.startingHealth;
    }
}
