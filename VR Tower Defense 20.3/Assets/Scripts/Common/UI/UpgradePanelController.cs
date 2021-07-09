using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = System.Object;

public class UpgradePanelController : MonoBehaviour
{
    [Serializable]
    public class ButtonMeta
    {
        public UpgradeCard card;
        public TextMeshProUGUI propertyValueField;
        public TextMeshProUGUI propertyUpgradeField;

        public UpgradeCard.FormatTypes fieldFormat;
        public string prefix;
        public string postfix;

        public UpgradeCard.FormatTypes upgradeFormat;
        public string upgradePrefix;
        public string upgradePostfix;
    }
    
    public GameData gameData;
    public GameObject cardGrid;
    public GameObject buttonPrefab;
    //
    public GameObject upgradePanel;
    //
    // public CEvent_UpgradeCard upgradeButtonClickEvent;

    [Tooltip("The name of the scriptable object that represents this item's upgrades")]
    private Object upgradeAsset;
    public List<ButtonMeta> cardGroup = new List<ButtonMeta>();

    private UpgradeCard _activeUpgradeCard;
    
    // [Header("Upgrade Cards")]
    // public UpgradeCard damageCard;
    // public UpgradeCard velocityCard;
    // public UpgradeCard rotationSpeedCard;
    // public UpgradeCard accuracyCard;
    //
    // [Header("Attribute Value Fields")]
    // public TextMeshProUGUI bulletDamageTMP;
    // public TextMeshProUGUI bulletVelocityTMP;
    // public TextMeshProUGUI towerRotationSpeedTMP;
    // public TextMeshProUGUI bulletAccuracyTMP;
    //
    // [Header("Attribute Upgrade Values Fields")]
    // public TextMeshProUGUI bulletDamageUpgradeTMP;
    // public TextMeshProUGUI bulletVelocityUpgradeTMP;
    // public TextMeshProUGUI towerRotationSpeedUpgradeTMP;
    // public TextMeshProUGUI bulletAccuracyUpgradeTMP;

    // Start is called before the first frame update
    void Start()
    {

        // CreateUpgradeButton(damageCard);
        // CreateUpgradeButton(velocityCard);
        // CreateUpgradeButton(rotationSpeedCard);
        // CreateUpgradeButton(accuracyCard);

        foreach (var button in cardGroup)
        {
            Debug.Log(button.card);
            CreateUpgradeButton(button.card);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var button in cardGroup)
        {
            button.propertyValueField.text = button.prefix + button.card.getCurrentValue().ToString(UpgradeCard.FormatTypeStrings[button.fieldFormat]) + button.postfix;
        }
        
        // bulletDamageTMP.text = upgrades.Damage.ToString("n0");
        // bulletVelocityTMP.text = "x" + upgrades.BulletVelocityModifier.ToString("n0");
        // towerRotationSpeedTMP.text = upgrades.TowerRotationSpeed.ToString("n0") + "°/s";
        // bulletAccuracyTMP.text = "±" + upgrades.TowerRotationSpeed.ToString("n0") + "°";
    }
    
    private void CreateUpgradeButton(UpgradeCard info)
    {
        if (info.maxUpgradeReached && !info.createIfMaxUpgradeReachedOnStartup) return;
        
        GameObject card = Instantiate(buttonPrefab, cardGrid.transform.position, buttonPrefab.transform.rotation);
        card.GetComponent<Button>().onClick.AddListener( () => { ShowUpgradeCard(info); } );
        card.transform.SetParent(cardGrid.transform);
        info.buttonInstance = card;
        
        if ( info.maxUpgradeReached ) card.SetActive(false);
    
        Debug.Log(info.updateCard);
        
        info.updateCard();
        
        UpgradeButtonInit(card, info);
    }
    
    private void UpgradeButtonInit(GameObject card, UpgradeCard info)
    {
        card.transform.Find("Upgrade Name").GetComponent<TextMeshProUGUI>().text = info.upgradeName;
        card.GetComponent<UpgradeButtonClicked>().upgradeCardInfo = info;
        // card.transform.Find("Upgrade Cost").GetComponent<TextMeshProUGUI>().text = "C. " + info.upgradeCost;
    }
    
    public void ShowUpgradeCard(UpgradeCard info)
    {
        if (info == null) return;
        
        // clear the upgrade values from the info panel

        foreach (var button in cardGroup)
        {
            button.propertyUpgradeField.text = "";
        }
        
        // bulletDamageUpgradeTMP.text = "";
        // bulletVelocityUpgradeTMP.text = "";
        // towerRotationSpeedUpgradeTMP.text = "";
        // bulletAccuracyUpgradeTMP.text = "";
        
        // check if the max upgrade has been reached for this card
        // if it has, hide the panel
        if (info.maxUpgradeReached)
        {
            upgradePanel.SetActive(false);
            _activeUpgradeCard = null;
            return;
        }
        
        if (_activeUpgradeCard != info)
        {
            upgradePanel.transform.Find("Purchase Button").GetComponent<ButtonEvents>().DeselectButton();
        }
        
        // otherwise, show the panel and set the current card
        upgradePanel.SetActive(true);
        _activeUpgradeCard = info;
    
        Debug.Log(info);
        
        // set the panel header and description
        upgradePanel.transform.Find("Header").GetComponent<TextMeshProUGUI>().text = info.upgradeName;
        upgradePanel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = info.upgradeDescription;
        
        // get the puchase button and it's onclick action
        var purchaseButton = upgradePanel.transform.Find("Purchase Button");
        var purchaseButtonOnClick = purchaseButton.GetComponent<Button>().onClick;
        
        // remove all the listeners so that we dont keep adding the same one multiple times
        // then re add the function that will call this function again to make sure the panel dissapears if we reach
        // the max upgrade for this card
        purchaseButtonOnClick.RemoveAllListeners();
        purchaseButtonOnClick.AddListener(PurchaseButtonClicked);
        
        // set the cost value in the panel
        upgradePanel.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "C. " + info.upgradeCost.ToString("n0");
    
        // check if the player has enough credit to purchase the upgrade
        if (gameData.gold >= info.upgradeCost)
        {
            // if they do have enough credit, show the button and hide the no credit warning
            purchaseButton.gameObject.SetActive(true); 
            purchaseButtonOnClick.AddListener(info.purchase);
            upgradePanel.transform.Find("Fund Warning").GetComponent<TextMeshProUGUI>().gameObject.SetActive(false);
        }
        else
        {
            // if they do not have enough credits to buy the upgrade, hide the purchase button and show the fund warning
            purchaseButton.gameObject.SetActive(false);
            upgradePanel.transform.Find("Fund Warning").GetComponent<TextMeshProUGUI>().gameObject.SetActive(true);
        }
    
        // switch on the upgrade type of the card so that we can show which attribute is being upgraded
        foreach (var button in cardGroup)
        {
            if (button.card.Equals(info))
            {
                button.propertyUpgradeField.text = button.upgradePrefix +
                                                   (info.getUpgradeValue() - info.getCurrentValue()).ToString(UpgradeCard.FormatTypeStrings[button.fieldFormat]) +
                                                   button.upgradePostfix;
                break;
            }   
        }
        // switch (info.upgradeType)
        // {
        //     case "Damage":
        //         bulletDamageUpgradeTMP.text = "+" + (info.getUpgradeValue() - upgrades.Damage ).ToString("n0");
        //         break;
        //     case "Velocity":
        //         bulletVelocityUpgradeTMP.text = "+" + (info.getUpgradeValue() - upgrades.BulletVelocityModifier).ToString("n0");
        //         break;
        //     case "Rotation":
        //         towerRotationSpeedUpgradeTMP.text = "+" + (info.getUpgradeValue() - upgrades.TowerRotationSpeed).ToString("n0");
        //         break;
        //     case "Accuracy":
        //         bulletAccuracyUpgradeTMP.text = "-" + (info.getUpgradeValue() - upgrades.BulletAccuracy).ToString("n2") + "°";
        //         break;
        // }
    }
    
    public void HideUpgradeCard()
    {
        upgradePanel.transform.Find("Purchase Button").GetComponent<ButtonEvents>().DeselectButton();
        upgradePanel.SetActive(false);
    }
    
    private void PurchaseButtonClicked()
    {
        StartCoroutine(DelayUpgradeCard());
    }
    
    IEnumerator DelayUpgradeCard()
    {
        yield return new WaitForSeconds(0.2f);
        ShowUpgradeCard(_activeUpgradeCard);
    }

}
