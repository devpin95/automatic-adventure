using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WallUpgradeController : MonoBehaviour
{
    public WallUpgrades wallUpgrades;
    public GameData gameData;
    public GameObject cardGrid;
    public GameObject buttonPrefab;

    public GameObject upgradePanel;

    public UpgradeCard repairCard;
    public UpgradeCard healthUpgradeCard;
    private UpgradeCard _activeUpgradeCard;

    public TextMeshProUGUI wallCurrentHealthTMP;
    public TextMeshProUGUI wallMaxHealthTMP;

    public TextMeshProUGUI wallCurrentHealthUpgradeTMP;
    public TextMeshProUGUI wallMaxHealthUpgradeTMP;

    // Start is called before the first frame update
    void Start()
    {
        // repairCard.upgradeCost = wallUpgrades.costToRepair100;
        // healthUpgradeCard.upgradeCost = wallUpgrades.wallUpgradeHealthCosts[wallUpgrades.wallUpgradeCount];

        // wallUpgrades.UpdateWallCurrentHealthCard(repairCard);
        // wallUpgrades.UpdateWallMaxHealthCard(healthUpgradeCard);

        CreateUpgradeButton(repairCard);
        CreateUpgradeButton(healthUpgradeCard);
    }

    // Update is called once per frame
    void Update()
    {
        wallCurrentHealthTMP.text = gameData.wallCurrentHealth.ToString();
        wallMaxHealthTMP.text = gameData.wallMaxHealth.ToString();
    }

    private void CreateUpgradeButton(UpgradeCard info)
    {
        if (info.maxUpgradeReached && !info.createIfMaxUpgradeReachedOnStartup) return;
        
        GameObject card = Instantiate(buttonPrefab, cardGrid.transform.position, buttonPrefab.transform.rotation);
        card.transform.SetParent(cardGrid.transform);
        info.buttonInstance = card;
        
        if ( info.maxUpgradeReached ) card.SetActive(false);

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
        wallCurrentHealthUpgradeTMP.text = "";
        wallMaxHealthUpgradeTMP.text = "";
        
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
        upgradePanel.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "C. " + info.upgradeCost;

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
        switch (info.upgradeType)
        {
            case "Max Health":
                wallMaxHealthUpgradeTMP.text = "+" + (info.getUpgradeValue() - gameData.wallMaxHealth);
                break;
            case "Current Health":
                wallCurrentHealthUpgradeTMP.text = "+" + info.getUpgradeValue();
                break;
        }
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
