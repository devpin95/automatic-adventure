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
    public bool isTracer;
    public float tracerBrightness;
    
    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)]
    public Color defaultTracerColor = Color.yellow;
    
    private float live = 0.0f;
    private float killBy = 4.0f;
    private float ricochetChance = 0.175f;
    private float ricochetSpeed = 5f;
    private bool ricocheting = false;
    private TrailRenderer _trailRenderer;
    private Light _tracerLight;

    private Material _materialInstance;

    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _trailRenderer = GameObject.Find("Trail").GetComponent<TrailRenderer>();
        _tracerLight = GameObject.Find("Tracer Light").GetComponent<Light>();
        
        Renderer renderer = GetComponent<Renderer>();
        _materialInstance = Instantiate(renderer.material);
        renderer.material = _materialInstance;
        _trailRenderer.material = _materialInstance;
        TracerColor(defaultTracerColor);
    }

    // Update is called once per frame
    void Update()
    {
        live += Time.deltaTime;

        if (live > killBy)
        {
            // this is a pooled object, so set active to false instead of destroying
            DeactivatePooledObject();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            GameObject puff = BulletImpactPool.Instance.GetPooledObject();

            if (puff != null)
            {
                puff.transform.position = transform.position;
                ParticleSystem ps = puff.transform.Find("Puff").GetComponent<ParticleSystem>();
                ps.Clear();
                ps.Play();
                puff.SetActive(true);
            }

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
                    // this is a pooled object, so set active to false instead of destroying
                    DeactivatePooledObject();
                }
            }
            else
            {
                DeactivatePooledObject();
            }
        }
        else if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Ball Launcher"))
        {
            EnemyEventController eventController = other.gameObject.GetComponent<EnemyEventController>();
            if (eventController)
            {
                eventController.directHitEvent.Invoke(attributes.damage);
            }
        }
        else
        {
            // this is a pooled object, so set active to false instead of destroying
            DeactivatePooledObject();
        }
    }

    private void DeactivatePooledObject()
    {
        live = 0.0f;
        killBy = 4.0f;
        ricocheting = false;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        // rb.angularDrag = 0.0F;
        rb.angularVelocity = Vector3.zero;
        _trailRenderer.enabled = false;
        _tracerLight.intensity = 0;
        isTracer = false;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _trailRenderer.Clear();
        if (isTracer)
        {
            StartCoroutine(DelayedTracerTrail());
        }
    }

    IEnumerator DelayedTracerTrail()
    {
        // float randDelay = UnityEngine.Random.Range(0.01f, 0.05f);
        float randDelay = 0.01f;
        yield return new WaitForSeconds(randDelay);
        _trailRenderer.Clear();
        _trailRenderer.enabled = true;
        _tracerLight.intensity = tracerBrightness;
    }

    public void MakeTracerRound()
    {
        _trailRenderer.enabled = true;
        canRicochet = true;
        isTracer = true;
    }

    public void MakeDefaultRound()
    {
        canRicochet = false;
        isTracer = false;
    }

    public void TracerColor(Color color)
    {
        _materialInstance.SetColor("Color_190eba34eefa4e28a2fa1387cf5bbe85", color);
    }
}
