using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public ProjectileAttributes attributes;
    public GameObject particles;
    public bool canRicochet;
    private float live = 0.0f;
    private float killBy = 4.0f;
    private float ricochetChance = 0.175f;
    private float ricochetSpeed = 10f;
    private bool ricocheting = false;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        live += Time.deltaTime;

        if (live > killBy)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            GameObject puff = Instantiate(particles, transform.position, particles.transform.rotation);

            if (!ricocheting)
            {
                float chance = UnityEngine.Random.Range(0.0f, 1.0f);
                if (canRicochet && chance < ricochetChance)
                {
                    ricocheting = true;
                    rb.velocity = Vector3.zero;
                    rb.angularDrag = 0.0F;

                    float randXrot = UnityEngine.Random.Range(0, 45);
                    float randZrot = UnityEngine.Random.Range(-45f, 45f);
                    transform.rotation = Quaternion.Euler(randXrot, 0, randZrot);
                    
                    rb.AddForce(transform.up * ricochetSpeed, ForceMode.Impulse);

                    live = 0.0f;
                    killBy = 2.5f;
                    
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyEventController eventController = other.gameObject.GetComponent<EnemyEventController>();
            if (eventController)
            {
                eventController.directHitEvent.Invoke(attributes.damage);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
