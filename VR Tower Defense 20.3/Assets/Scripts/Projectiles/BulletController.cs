using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    private float live = 0.0f;
    private float killBy = 4.0f;
    private float ricochetChance = 0.175f;
    private float ricochetSpeed = 40f;
    private bool ricocheting = false;
    public GameObject particles;

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
            ParticleSystem ps = puff.GetComponent<ParticleSystem>();
            
            ParticleSystem.ShapeModule sm = ps.shape;
            sm.radius = UnityEngine.Random.Range(0.05f, 0.1f);
            
            ps.startSpeed = UnityEngine.Random.Range(0.1f, 3.0f);
            
            ps.Play();
            
            if (!ricocheting)
            {
                float chance = UnityEngine.Random.Range(0.0f, 1.0f);
                if (chance < ricochetChance)
                {
                    ricocheting = true;
                    rb.velocity = Vector3.zero;
                    rb.angularDrag = 0.0F;

                    // Vector3 pos = transform.position;
                    // pos.y += 0.1f;
                    // transform.position = pos;

                    float randXrot = UnityEngine.Random.Range(0, 45);
                    // float randYrot = UnityEngine.Random.Range(45f, 135f);
                    float randZrot = UnityEngine.Random.Range(-45f, 45f);
                    transform.rotation = Quaternion.Euler(randXrot, 0, randZrot);
                    
                    rb.AddForce(transform.up * ricochetSpeed, ForceMode.Impulse);

                    live = 0.0f;
                    killBy = 2.5f;

                    // GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    // bullet.GetComponent<Rigidbody>().AddForce(transform.forward * ricochetSpeed, ForceMode.Impulse);

                    print("Ricochet!");
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
