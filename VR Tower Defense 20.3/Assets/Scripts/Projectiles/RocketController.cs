using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public ProjectileAttributes attributes;
    public RocketLauncherUpgrades upgrades;
    
    private float rocketSpeedModifier;
    
    private float rocketDamageRadius;
    private float rocketAddForceRadius;
    
    public float rocketPower;
    public GameObject explosionPrefab;
    public LayerMask rocketImpactLayers;
    private Vector3 _startingPoint;
    private Vector3 _impactPoint;
    private float speedOfSound = 343;
    private bool _active = true;

    // Start is called before the first frame update
    void Start()
    {
        _startingPoint = transform.position;
        rocketDamageRadius = upgrades.ExplosionRadius;
        rocketAddForceRadius = rocketDamageRadius + 5;
        rocketSpeedModifier = upgrades.Velocity;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, Mathf.Infinity, rocketImpactLayers))
        {
            _impactPoint = hit.point;
        }
    }

    void Update()
    {
        if (!_active) return;
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
        if (!_active) return;
        Explode();
    }
    
    private void Explode()
    {
        Collider[] fullDamageHits = Physics.OverlapSphere(transform.position, rocketDamageRadius, rocketImpactLayers);
        foreach (var hittable in fullDamageHits)
        {
            if (hittable.CompareTag("Enemy") || hittable.CompareTag("Ball Launcher"))
            {
                EnemyEventController hitController = hittable.GetComponent<EnemyEventController>();

                if (hitController)
                {
                    hitController.indirectHitEvent.Invoke(attributes.damage);
                }
            }
        }
        
        Collider[] pushhits = Physics.OverlapSphere(transform.position, rocketAddForceRadius, rocketImpactLayers);
        foreach (var hittable in pushhits)
        {
            if (hittable.CompareTag("Enemy") || hittable.CompareTag("Ball Launcher"))
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
                    hittable.GetComponent<EnemyEventController>().indirectHitEvent.Invoke(Mathf.Abs(damage));
                }
                hittable.transform.GetComponent<Rigidbody>()
                    .AddExplosionForce(rocketPower, transform.position, rocketAddForceRadius, 3.0f);
            }
        }

        // Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);

        GameObject explosion = RocketExplosionPool.Instance.GetPooledObject();
        if (explosion)
        {
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
        
            float dis = Vector3.Distance(transform.position, Camera.main.transform.position);
            float soundDelay = dis / speedOfSound;

            StartCoroutine(DisableDelay(soundDelay));
        }
        _active = false;
    }

    IEnumerator DisableDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
