using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareController : MonoBehaviour
{
    public CEvent fireFlare;
    private Rigidbody _rb;
    private bool _grounded = false;
    private bool _dimming = false;
    private bool _falling = false;
    private float _lightTime;
    private Light _flare;
    private float _dimRate = 0.5f;

    public Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _lightTime = UnityEngine.Random.Range(30, 45);
        StartCoroutine(FlareDim());
        _flare = transform.Find("Light").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = _rb.velocity;
        
        if (velocity.y < 0) _falling = true;

        if (!_grounded)
        {
            // _rb.AddForce(Vector3.up * 4.9f, ForceMode.Force);
            if (velocity.y < -1.5f)
            {
                float newXVel = Mathf.Clamp(velocity.x - 0.05f, 0, velocity.x);
                float newZVel = Mathf.Clamp(velocity.z - 0.05f, 0, velocity.z);
                _rb.velocity = new Vector3(newXVel, -1.5f, newZVel);
            }
        }
        
        if (_dimming)
        {
            _flare.intensity -= _dimRate;

            if (_flare.intensity == 0)
            {
                fireFlare.Raise();
                Destroy(gameObject);
            }
        }

        velocity = _rb.velocity;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            _grounded = true;
            _rb.velocity = Vector3.zero;
        }
    }

    IEnumerator FlareDim()
    {
        yield return new WaitForSeconds(_lightTime);
        _dimming = true;
    }
}
