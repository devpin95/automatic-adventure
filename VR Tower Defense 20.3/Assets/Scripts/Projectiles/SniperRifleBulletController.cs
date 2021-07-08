using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifleBulletController : MonoBehaviour
{
    public ProjectileAttributes attributes;

    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)]
    public Color defaultTracerColor = Color.red;

    private Rigidbody _rb;
    private TrailRenderer _trailRenderer;
    private Light _tracerLight;
    private Material _materialInstance;
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _trailRenderer = GameObject.Find("Trail").GetComponent<TrailRenderer>();
        _tracerLight = GameObject.Find("Tracer Light").GetComponent<Light>();
        
        Renderer renderer = GetComponent<Renderer>();
        _materialInstance = Instantiate(renderer.material);
        renderer.material = _materialInstance;
        _trailRenderer.material = _materialInstance;
        TracerColor(defaultTracerColor);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TracerColor(Color color)
    {
        _materialInstance.SetColor("Color_190eba34eefa4e28a2fa1387cf5bbe85", color);
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
        }
        else
        {
            EnemyEventController eventController = other.gameObject.GetComponent<EnemyEventController>();
            if (eventController)
            {
                eventController.directHitEvent.Invoke(attributes.damage, attributes.targetType);
            }
        }
        
        Destroy(gameObject);
    }
}
