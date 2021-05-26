using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHudController : MonoBehaviour
{
    public GameObject nonPhysicalHudElements;
    public GameObject physicalHudEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainHudState(bool state)
    {
        if (state)
        {
            nonPhysicalHudElements.SetActive(true);
            physicalHudEffect.SetActive(true);
        }
        else
        {
            nonPhysicalHudElements.SetActive(false);
            physicalHudEffect.SetActive(false);
        }
    }
}
