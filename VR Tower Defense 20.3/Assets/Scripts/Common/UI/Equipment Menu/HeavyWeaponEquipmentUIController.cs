using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeavyWeaponEquipmentUIController : MonoBehaviour
{
    public MainTowerAttributes towerAttributes;
    private HeavyWeaponCard _activeHeavyWeapon = null;

    [Header("Heavy Weapon Equipment")] 
    public Button heavyWeaponButton;
    public Image heavyWeaponButtonImage;
    public TextMeshProUGUI heavyWeaponNameField;
    public TextMeshProUGUI heavyWeaponTypeField;
    public TextMeshProUGUI heavyWeaponDamageField;

    [Header("Empty Slot Button Sprites")]
    public Sprite emptySlotSpriteDefault;
    
    private bool _slotEmpty = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _activeHeavyWeapon = towerAttributes.heavyWeaponCard;
        if (_activeHeavyWeapon == null) _slotEmpty = true;
        ShowHeavyWeaponCard();
        
        heavyWeaponButton.onClick.AddListener(HeavyWeaponSlotClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (_activeHeavyWeapon != towerAttributes.heavyWeaponCard)
        {
            // do stuff to upgrade the card
            _activeHeavyWeapon = towerAttributes.heavyWeaponCard;
            ShowHeavyWeaponCard();
        }
    }

    private void ShowHeavyWeaponCard()
    {
        if (_activeHeavyWeapon == null)
        {
            heavyWeaponNameField.text = "Slot Empty";
            heavyWeaponButtonImage.sprite = emptySlotSpriteDefault;
            heavyWeaponTypeField.text = "";
            heavyWeaponDamageField.text = "";
        }
        else
        {
            heavyWeaponNameField.text = _activeHeavyWeapon.weaponName;
            heavyWeaponButtonImage.sprite = _activeHeavyWeapon.itemPreview;
            
            var weaponType = _activeHeavyWeapon.heavyWeaponType;
            heavyWeaponTypeField.text = HeavyWeaponCard.HeavyWeaponTypeNames[weaponType];
            
            heavyWeaponDamageField.text = _activeHeavyWeapon.currentDamage.ToString("n0");
        }
    }

    public void HeavyWeaponSlotClicked()
    {
        Debug.Log("Heavy Weapon Button Clicked");
    }
}
