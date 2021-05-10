using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool lookOpposite = false;
    private Transform cameraTrans;
    private RectTransform _rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTrans = Camera.main.transform;
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lookOpposite)
        {
            transform.LookAt(2 * transform.position - cameraTrans.position);
        }
        else
        {
            transform.LookAt(cameraTrans);
        }
    } 
}
