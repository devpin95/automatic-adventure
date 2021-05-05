using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBallController : MonoBehaviour
{
    public GameObject impactParticles;
    public CEvent_Float hitWall;
    public EnemyAttributes attributes;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Enemy")) return;
        else if (other.transform.CompareTag("Wall"))
        {
            // other.transform.GetComponent<WallManager>().HitWall(hitDamage);
            hitWall.Raise(attributes.wallHitDamage);
        }
        
        var impact = Instantiate(impactParticles, transform.position,
            impactParticles.transform.rotation * Quaternion.Euler(90, 0, 0));
        impact.transform.localScale = new Vector3(4f, 9f, 4f);
        
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
