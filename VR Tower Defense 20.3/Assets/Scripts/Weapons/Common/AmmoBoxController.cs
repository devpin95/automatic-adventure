using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoBoxController : MonoBehaviour
{
    public MachineGunAmmoBox ammoBox;
    public TextMeshProUGUI ammoIndicator;
    private int count;
    private int capacity;

    public int Count
    {
        get => count;
        set => count = value;
    }

    public int Capacity
    {
        get => capacity;
        set => capacity = value;
    }

    private void Awake()
    {
        Count = ammoBox.count;
        Capacity = ammoBox.capacity;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ammoIndicator.text = Count.ToString();
    }

    public void LoadAmmo(int count)
    {
        Count += count;
        if (Count > Capacity) Count = Capacity;
    }
}
