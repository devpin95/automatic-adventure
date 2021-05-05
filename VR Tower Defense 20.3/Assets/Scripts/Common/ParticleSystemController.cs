using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    private List<float> psdur = new List<float>();
    private float max_dur;
    private bool _hasAudioSource = false;
    private AudioSource _audioSource;
    private float speedOfSound = 343; // m/s
    
    // Start is called before the first frame update
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource)
        {
            _hasAudioSource = true;
            Debug.Log("Has Audio Source");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        
        foreach (var ps in systems)
        {
            var main = ps.main;
            
            psdur.Add(main.duration);
        }
        
        max_dur = Mathf.Max(psdur.ToArray());

        if (_hasAudioSource)
        {
            float dis = Vector3.Distance(transform.position, Camera.main.transform.position);
            float soundDelay = dis / speedOfSound;

            if (max_dur < soundDelay + _audioSource.clip.length)
            {
                max_dur = soundDelay + _audioSource.clip.length;
            }
            
            StartCoroutine(SoundTimeDelay(soundDelay));
        }

        StartCoroutine(DisableDelay(max_dur));
    }

    IEnumerator DisableDelay(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    IEnumerator SoundTimeDelay(float time)
    {
        yield return new WaitForSeconds(time);
        _audioSource.Play();
    }
}
