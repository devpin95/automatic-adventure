using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public ProjectileAttributes attributes;
    public float rocketSpeedModifier;
    public float rocketDamageRadius;
    public float rocketAddForceRadius;
    public float rocketPower;
    public GameObject explosionPrefab;
    public LayerMask rocketImpactLayers;
    private Vector3 _startingPoint;
    private Vector3 _impactPoint;

    // Start is called before the first frame update
    void Start()
    {
        _startingPoint = transform.position;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, Mathf.Infinity, rocketImpactLayers))
        {
            _impactPoint = hit.point;
        }
    }

    void Update()
    {
        transform.position =
            Vector3.MoveTowards(transform.position, _impactPoint, Time.deltaTime * rocketSpeedModifier);

        if (transform.position == _impactPoint)
        {
            Explode();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube( _impactPoint, new Vector3(1, 1, 1) );
    }

    private void OnCollisionEnter(Collision other)
    {
        Explode();
    }
    
    private void Explode()
    {
        Collider[] killhits = Physics.OverlapSphere(transform.position, rocketDamageRadius, rocketImpactLayers);
        foreach (var hittable in killhits)
        {
            if (hittable.CompareTag("Enemy"))
            {
                EnemyEventController hitController = hittable.GetComponent<EnemyEventController>();

                if (hitController)
                {
                    hitController.indirectHitEvent.Invoke(100f);
                }
                
                Destroy(hittable.gameObject);
            }
        }
        
        Collider[] pushhits = Physics.OverlapSphere(transform.position, rocketAddForceRadius, rocketImpactLayers);
        foreach (var hittable in pushhits)
        {
            if (hittable.CompareTag("Enemy"))
            {
                if (hittable.GetComponent<EnemyEventController>())
                {
                    // get the distance from the full damage zone to the add force zone
                    // get the distance of the hittable from the center of the explosion
                    // get distance from the full damage zone
                    // get the percentage of the damage to apply
                    float range = rocketAddForceRadius - rocketDamageRadius;
                    float distance = Vector3.Distance(transform.position, hittable.transform.position);
                    float proportion = Mathf.Abs(distance - rocketDamageRadius);
                    float damage = (proportion / range) * attributes.damage;
                    hittable.GetComponent<EnemyEventController>().indirectHitEvent.Invoke(damage);
                }
                hittable.transform.GetComponent<Rigidbody>()
                    .AddExplosionForce(rocketPower, transform.position, rocketAddForceRadius, 3.0f);
            }
        }

        Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
        
        Destroy(gameObject);
    }
}
