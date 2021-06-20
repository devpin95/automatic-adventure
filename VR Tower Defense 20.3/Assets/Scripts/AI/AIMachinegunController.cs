using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMachinegunController : MonoBehaviour
{
    public float velocityModifier = 12;
    public float range;
    public LayerMask targetLayer;

    public GameObject bulletPrefab;
    private Rigidbody bulletPrefabRb;
    
    public float fireDelay = 0.1f;
    public int tracerSpacing = 6;
    
    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)]
    public Color tracerColor = Color.yellow;
    
    private float _fireDelta = 0;
    private int _fireCount = 0;

    private float _timeCounter = 0;
    private float _timeTrackingTarget = 0;
    private float _maxTimeToTrackTarget = 4;
    private Vector3 _aimOffset = Vector3.zero;
    private GameObject _target;
    private Rigidbody _targetRb;
    private Transform _firePoint;
    private bool _targetAcquired = false;
    
    private Vector3 _aimPoint = Vector3.zero;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _firePoint = transform.Find("Fire Point").GetComponent<Transform>();
        bulletPrefabRb = bulletPrefab.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _timeCounter += Time.deltaTime;
        
        if (_targetAcquired)
        {
            _timeTrackingTarget += Time.deltaTime;
            float disToTarget = Vector3.Distance(transform.position, _target.transform.position);
            
            if (!_target.gameObject.activeInHierarchy || _timeTrackingTarget > _maxTimeToTrackTarget || disToTarget > range)
            {
                // we need to look for a new target
                _targetAcquired = false;
                _target = null;
                _aimPoint = Vector3.zero;
                _timeTrackingTarget = 0;
                return;
            }

            Aim();

            _fireDelta += Time.deltaTime;
            if (_fireDelta > fireDelay)
            {
                _fireDelta = 0;
                Fire();   
            }
        }
        else
        {
            AcquireTarget();
        }
    }

    private void AcquireTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, range, targetLayer);

        if (possibleTargets.Length > 0)
        {
            for (int i = 0; i < possibleTargets.Length; ++i)
            {
                _targetAcquired = true;
                _target = possibleTargets[0].gameObject;
                _targetRb = _target.GetComponent<Rigidbody>();

                if (_targetRb) break;
            }
        }
    }

    private void Fire()
    {
        GameObject bullet = MachineGunBulletPool.SharedInstance.GetPooledObject();

        if (bullet == null) return;
        
        var trail = bullet.transform.Find("Trail").GetComponent<TrailRenderer>();
        trail.enabled = false;
        
        bullet.transform.position = _firePoint.position;
        bullet.transform.rotation = transform.rotation;
        // bullet.transform.rotation = Quaternion.AngleAxis(90, bullet.transform.right);

        ++_fireCount;
        float ranXrot = UnityEngine.Random.Range(-4.0f, 4.0f);
        float ranYrot = UnityEngine.Random.Range(-4.0f, 4.0f);
        Vector3 randDir = Quaternion.Euler(ranXrot, ranYrot,  0) * _firePoint.forward;
        
        BulletController bulletController = bullet.GetComponent<BulletController>();

        //GameObject bullet;
        if (_fireCount % tracerSpacing == 0)
        {
            bulletController.MakeTracerRound();
            bulletController.TracerColor(tracerColor);
        }
        else bulletController.MakeDefaultRound();
        
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().AddForce(randDir.normalized * velocityModifier, ForceMode.Impulse);
    }

    private void Aim()
    {
        Vector3 expectedForce = Vector3.forward * velocityModifier;
        Vector3 expectedBulletVelocity = (expectedForce / bulletPrefabRb.mass) * Time.fixedDeltaTime;
        float expectedBulletSpeed = expectedBulletVelocity.magnitude;

        Vector3 futureTargetPos = _targetRb.velocity + _target.transform.position * Time.fixedDeltaTime;
        
        _aimOffset = futureTargetPos.normalized * Mathf.Sin(_timeCounter * 10) * 3;
        
        float flightTime = Vector3.Distance(transform.position, futureTargetPos) / expectedBulletSpeed;

        _aimPoint = futureTargetPos + _target.transform.position + _aimOffset;

        // Vector3 aimAt = (_aimPoint - transform.position).normalized;
        
        // Vector3 vel = _target.GetComponent<Rigidbody>().velocity;
        transform.LookAt(_aimPoint);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_aimPoint, 1);
    }
}
