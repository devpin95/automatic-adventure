using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AmmoBoxLoaderController : MonoBehaviour
{
    public int ammoLoadCount = 10;
    public float ammoLoadRate = 1.5f;
    private AmmoBoxController _ammoBox;
    private bool waitingForPayload = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitingForPayload) StartCoroutine(LoadAmmoBox());
    }

    public void OnAmmoBoxSocketed(XRBaseInteractable box)
    {
        _ammoBox = box.GetComponent<AmmoBoxController>();
        // Debug.Log(_ammoBox + " loaded");
    }

    public void OnAmmoBoxUnsocketed(XRBaseInteractable box)
    {
        _ammoBox = null;
        // Debug.Log("ammo box unloaded");
    }

    IEnumerator LoadAmmoBox()
    {
        waitingForPayload = true;
        yield return new WaitForSeconds(ammoLoadRate);
        _ammoBox?.LoadAmmo(ammoLoadCount);
        waitingForPayload = false;
    }
}
