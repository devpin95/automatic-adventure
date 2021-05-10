using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLightController : MonoBehaviour
{
    public GameObject greenLightObj;
    public GameObject redLightObj;
    public GameObject yellowLightObj;

    public string greenLightMaterialTimerVariable;
    public string yellowLightMaterialTimerVariable;
    public string redLightMaterialTimerVariable;

    private float timer = 0.0f;
    private Material greenLightM;
    private Material redLightM;
    private Material yellowLightM;

    private float lightFadeSpeed = 0.2f;
    private float wallHitCountdownTimer = 5.0f;
    private float wallHitCountdownStartTime = 5.0f;
    private bool wallHit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        greenLightM = greenLightObj.GetComponent<Renderer>().material;
        redLightM = redLightObj.GetComponent<Renderer>().material;
        yellowLightM = yellowLightObj.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * 10;

        if (wallHit)
        {
            wallHitCountdownTimer -= Time.deltaTime;

            if (wallHitCountdownTimer <= 0)
            {
                StartCoroutine(TurnOffLight(yellowLightM, yellowLightMaterialTimerVariable));
                wallHit = false;
            }
        }
    }

    public void WallIntegrityLow()
    {
        
    }

    public void WaveStarted()
    {
        Debug.Log("Wave Started For Light");
        StartCoroutine(TurnOnLight(greenLightM, greenLightMaterialTimerVariable));
        StartCoroutine(DelayTurnOffLight(greenLightM, greenLightMaterialTimerVariable, 5.0f));
    }

    public void WallUnderAttack()
    {
        wallHitCountdownTimer = wallHitCountdownStartTime;
        if (wallHit) {return;}
        
        wallHit = true;
        StartCoroutine(TurnOnLight(yellowLightM, yellowLightMaterialTimerVariable));
    }

    IEnumerator TurnOffLight(Material m, string prop)
    {
        float lerp = m.GetFloat(prop);

        while (lerp > 0.0)
        {
            m.SetFloat(prop, lerp);
            lerp -= lightFadeSpeed;
            yield return null;
        }
        
        m.SetFloat(prop, 0);
    }
    
    IEnumerator TurnOnLight( Material m, string prop)
    {
        float lerp = 0.0f;

        while (lerp < 1.0)
        {
            m.SetFloat(prop, lerp);
            lerp += lightFadeSpeed;
            yield return null;
        }
        
        m.SetFloat(prop, 1.0f);
    }

    IEnumerator DelayTurnOffLight(Material m, string prop, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(TurnOffLight(m, prop));
    }
}
