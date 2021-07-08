using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLauncherController : MonoBehaviour
{

    [Header("Movement")]
    public float drivingSpeed;
    public float stopDistance;
    
    [Header("Attacks")]
    public GameObject canonBallPrefab;
    public GameObject shotParticles;
    public Transform firePoint;
    public float shotRate;
    public float shotVelocityModifier;

    public CEvent_Int_Bool killed;
    
    public EnemyAttributes attributes;
    public float currentHealth;
    
    private float shotCounter = 0f;
    private Vector3 _target;
    private bool _state_approaching = true;
    private bool _state_at_distance = false;
    [SerializeField] private LayerMask _targetLayers;

    [SerializeField] private GameObject frontLeft;
    [SerializeField] private GameObject backLeft;
    [SerializeField] private GameObject frontRight;
    [SerializeField] private GameObject backRight;
    private float rotationSpeed = 5.0f;

    [SerializeField] private ParticleSystem damagedSmokeParticleSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        FindTarget();

        currentHealth = attributes.startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        
        if (_state_approaching)
        {
            // move the car
            transform.position =
                Vector3.MoveTowards(transform.position, _target, Time.deltaTime * drivingSpeed);

            // check distance from point
            if (Vector3.Distance(transform.position, _target) <= stopDistance)
            {
                _state_approaching = false;
                _state_at_distance = true;
            }
            
            // spin tires
            frontLeft.transform.Rotate(new Vector3(0, -rotationSpeed, 0));
            backLeft.transform.Rotate(new Vector3(0, -rotationSpeed, 0));
            frontRight.transform.Rotate(new Vector3(0, -rotationSpeed, 0));
            backRight.transform.Rotate(new Vector3(0, -rotationSpeed, 0));
        } 
        else if (_state_at_distance)
        {
            shotCounter += Time.deltaTime;

            if (shotCounter >= shotRate)
            {
                shotCounter = 0;
                Fire();
            }
        }
    }

    private void Fire()
    {
        float randomVelocity = UnityEngine.Random.Range(shotVelocityModifier - 500, shotVelocityModifier + 500);
        GameObject canonBall = CanonBallPool.Instance.GetPooledObject();

        if (canonBall)
        {
            canonBall.SetActive(true);
            canonBall.transform.position = firePoint.position;
            canonBall.transform.rotation = canonBall.transform.rotation;
            canonBall.GetComponent<Rigidbody>().AddForce(firePoint.forward * randomVelocity, ForceMode.Impulse);
        }
    }

    public void TakeIndirectHit(float damage, EnemyAttributes.EnemyType projectileTarget)
    {
        float actualDamage = damage;

        if (attributes.enemyType == projectileTarget) actualDamage *= GameModifiers.projectileTypeMatchModifier;
        else actualDamage *= GameModifiers.projectileTypeMismatchModifier;
        
        currentHealth -= actualDamage;

        if (currentHealth <= (attributes.startingHealth / 2))
        {
            damagedSmokeParticleSystem.Play();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(_target, new Vector3(1, 1, 1));
    }

    private void Die()
    {
        killed.Raise(attributes.EnemyValue, attributes.countAsEnemy);
        shotCounter = 0f;
        _state_approaching = true;
        _state_at_distance = false;
        damagedSmokeParticleSystem.Stop();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        currentHealth = attributes.startingHealth;
        FindTarget();
    }

    private void FindTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, _targetLayers))
        {
            _target = hit.point;
        }
    }
}
