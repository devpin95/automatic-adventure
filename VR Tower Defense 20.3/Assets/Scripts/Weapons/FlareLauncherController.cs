using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareLauncherController : MonoBehaviour
{
    public GameObject flarePrefab;
    [SerializeField] private Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        Fire();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        // Debug.Log("Firing flare!");
        GameObject flare = Instantiate(flarePrefab, firePoint.position, firePoint.rotation);
        flare.GetComponent<Rigidbody>().AddForce(firePoint.forward * 40, ForceMode.Impulse);
    }
}
