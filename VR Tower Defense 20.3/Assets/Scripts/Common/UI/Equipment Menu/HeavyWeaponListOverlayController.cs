using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeavyWeaponListOverlayController : MonoBehaviour
{
    public HeavyWeaponList heavyWeaponList;
    public MainTowerAttributes mainTowerAttributes;
    public GameObject heavyWeaponButtonPrefab;

    public GameObject itemGrid;

    [Header("Currently Equipped Preview")] 
    public TextMeshProUGUI currentWeaponName;
    public TextMeshProUGUI currentWeaponType;
    public TextMeshProUGUI currentWeaponDamage;
    public Image currentWeaponPreview;

    public Sprite weaponDefaultPreview;

    [Header("Switch-to Preview")] 
    public GameObject switchToPreview;
    public TextMeshProUGUI switchWeaponName;
    public TextMeshProUGUI switchWeaponType;
    public TextMeshProUGUI switchWeaponDamage;
    public Image switchWeaponPreview;
    public Button equipButton;
    
    private List<HeavyWeaponCard> _availableWeapons;
    private bool _initialized = false;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeOverlay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeOverlay()
    {
        if (_initialized) return;

        ClearList();
        
        if (mainTowerAttributes.heavyWeaponCard == null)
        {
            _availableWeapons = heavyWeaponList.heavyWeaponsCard;
            
            currentWeaponName.text = "Slot Empty";
            currentWeaponType.text = "";
            currentWeaponDamage.text = "";
            currentWeaponPreview.sprite = weaponDefaultPreview;
        }
        else
        {
            _availableWeapons = heavyWeaponList.heavyWeaponsCard
                .Where(t => t != mainTowerAttributes.heavyWeaponCard && t.purchased).ToList();
            
            currentWeaponName.text = mainTowerAttributes.heavyWeaponCard.weaponName;
            currentWeaponType.text = HeavyWeaponCard.HeavyWeaponTypeNames[mainTowerAttributes.heavyWeaponCard.heavyWeaponType];
            currentWeaponDamage.text = mainTowerAttributes.heavyWeaponCard.currentDamage.ToString("n0");
            currentWeaponPreview.sprite = mainTowerAttributes.heavyWeaponCard.itemPreview;
        }

        PopulateList();
        
        switchToPreview.SetActive(false);

        _initialized = true;
    }

    private void OnEnable()
    {
        InitializeOverlay();
    }

    private void OnDisable()
    {
        _initialized = false;
        Debug.Log("OnDisable() -> " + this.name);

        ClearList();
    }

    private void PopulateList()
    {
        Debug.Log("Populating list with " + _availableWeapons.Count + " items");
        foreach (var card in _availableWeapons)
        {
            Debug.Log("Creating button for " + card.weaponName);
            var cardButton = Instantiate(heavyWeaponButtonPrefab, itemGrid.transform.position, heavyWeaponButtonPrefab.transform.rotation);
            cardButton.transform.SetParent(itemGrid.transform);
            
            cardButton.GetComponent<Button>().onClick.AddListener(() => ItemButtonClicked(card));
            cardButton.transform.Find("Image").GetComponent<Image>().sprite = card.itemPreview;
        }
    }

    private void ItemButtonClicked(HeavyWeaponCard card)
    {
        Debug.Log("Button for item " + card.weaponName + " has been clicked");

        SetPreviewInformation(card);
        
        equipButton.onClick.RemoveAllListeners();
        var capturedCard = card;
        equipButton.onClick.AddListener(() => EquipWeapon(capturedCard));
        
        switchToPreview.SetActive(true);
    }

    private void SetPreviewInformation(HeavyWeaponCard card)
    {
        switchWeaponName.text = card.weaponName;
        switchWeaponType.text = HeavyWeaponCard.HeavyWeaponTypeNames[card.heavyWeaponType];
        switchWeaponDamage.text = card.currentDamage.ToString("n0");
        switchWeaponPreview.sprite = card.itemPreview;
    }

    private void EquipWeapon(HeavyWeaponCard card)
    {
        mainTowerAttributes.heavyWeaponCard = card;
        mainTowerAttributes.heavyWeaponPrefab = card.prefab;

        _initialized = false;
        InitializeOverlay();
    }

    private void ClearList()
    {
        for (int i = 0; i < itemGrid.transform.childCount; ++i)
        {
            Destroy(itemGrid.transform.GetChild(i).gameObject);
        }
    }
}
