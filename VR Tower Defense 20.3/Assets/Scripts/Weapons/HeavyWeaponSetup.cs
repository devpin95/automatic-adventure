using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class HeavyWeaponSetup : MonoBehaviour
{
    public Transform towerAttachPoint;
    public Transform heavyWeaponGuiAnchor;
    public MainTowerAttributes attributes;

    public GameObject guiParentObject;
    
    public string materialID; // Texture2D_389a2ea7d16c42ebbafab1c818a23307
    public Texture defaultCameraFeed;
    
    [FormerlySerializedAs("cameraFeed")] public CanvasRenderer cameraFeedRenderer;
    private GameObject heavyWeapon;
    private HeavyWeapon heavyWeaponData;

    private bool _weaponEquipped = true;
    
    // Start is called before the first frame update
    void Start()
    {
        CameraFeedInit();
        HeavyWeaponGUIInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CameraFeedInit()
    {
        GameObject prefab = attributes?.heavyWeaponPrefab;
        Texture delayedTexture = defaultCameraFeed;

        if (prefab)
        {
            heavyWeapon = Instantiate(prefab, towerAttachPoint.position, towerAttachPoint.rotation);
            heavyWeaponData = heavyWeapon.GetComponent<HeavyWeapon>();
            Vector3 offset = towerAttachPoint.position - heavyWeapon.GetComponent<HeavyWeapon>().AttachPoint();
            heavyWeapon.transform.position += offset;
            heavyWeapon.transform.SetParent(transform.parent.transform);

            delayedTexture = heavyWeaponData.renderTexture;
        }
        else
        {
            _weaponEquipped = false;
        }
        
        // for some reason, we need to wait before we set the material texture
        StartCoroutine(DelayedTexture(delayedTexture));
    }

    IEnumerator DelayedTexture(Texture texture)
    {
        yield return new WaitForSeconds(2);
        cameraFeedRenderer.GetMaterial().SetTexture(materialID, texture);
    }

    private void HeavyWeaponGUIInit()
    {
        if (!_weaponEquipped) return;
            
        GameObject gui = Instantiate(heavyWeaponData.gui, heavyWeaponGuiAnchor.position, heavyWeaponGuiAnchor.rotation);
        gui.transform.Rotate(0, 0, -90);
        gui.transform.SetParent(guiParentObject.transform);
    }
}
