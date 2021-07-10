using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeavyWeaponUpgradesUIController : MonoBehaviour
{
    [Serializable]
    public class HeavyWeaponGroup
    {
        public HeavyWeaponGroup(HeavyWeaponCard ncard, GameObject ngui)
        {
            card = ncard;
            gui = ngui;
        }
        
        public HeavyWeaponCard card;
        public GameObject gui;
    }

    private GameObject _activeMenu;
    
    [Header("Heavy Weapon List")] 
    public Transform heavyWeaponGrid;
    public GameObject heavyWeaponButtonPrefab;
    // public HeavyWeaponList heavyWeaponList;
    public GameObject emptyListIndicator;
    public GameObject menuPrompt;

    public List<HeavyWeaponGroup> groups = new List<HeavyWeaponGroup>();

    // Start is called before the first frame update
    void Start()
    {
        _activeMenu = menuPrompt;
        _activeMenu.SetActive(true);
        
        bool atLeastOneActiveCard = false;
        
        foreach (var group in groups)
        {
            if (group.card.purchased && group.card.isUpgradeable)
            {
                atLeastOneActiveCard = true;
                
                CreateHeavyWeaponButton(new HeavyWeaponGroup(group.card, group.gui));
            }
        }

        emptyListIndicator.SetActive(!atLeastOneActiveCard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject CreateHeavyWeaponButton(HeavyWeaponGroup group)
    {
        GameObject button = Instantiate(heavyWeaponButtonPrefab, heavyWeaponGrid.position,
            heavyWeaponGrid.rotation);
        button.transform.SetParent(heavyWeaponGrid);

        Button buttonComp = button.GetComponent<Button>();
        var capturedGroup = group;
        buttonComp.onClick.AddListener(() => { SwitchMenus(capturedGroup.gui); });

        button.transform.Find("Image").GetComponent<Image>().sprite = group.card.itemPreview;

        return button;
    }

    private void SwitchMenus(GameObject targetMenu)
    {
        _activeMenu.SetActive(false);
        targetMenu.SetActive(true);

        _activeMenu = targetMenu;
    }
}
