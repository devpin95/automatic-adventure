using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    private List<float> psdur = new List<float>();
    private float max_dur;
    
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();

        foreach (var ps in systems)
        {
            var main = ps.main;
            
            psdur.Add(main.duration);
        }

        max_dur = Mathf.Max(psdur.ToArray());
        Destroy(gameObject, max_dur + 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
