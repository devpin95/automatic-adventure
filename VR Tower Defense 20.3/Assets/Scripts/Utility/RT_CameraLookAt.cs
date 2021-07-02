using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RT_CameraLookAt : MonoBehaviour
{
    public GameObject target;

    public bool lookAway = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetTrans = lookAway ? (2 * transform.position - target.transform.position) : target.transform.position;
        transform.LookAt(targetTrans);
    }
}
