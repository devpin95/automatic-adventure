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

    // Start is called before the first frame update
    void Start()
    {
        repairCard.upgradeCost = wallUpgrades.costToRepair100;
        healthUpgradeCard.upgradeCost = wallUpgrades.wallUpgradeHealthCosts[wallUpgrades.wallUpgradeCount];

        wallUpgrades.UpdateWallCurrentHealthCard(repairCard);
        
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
        if (info.maxUpgradeReached) return;
        GameObject card = Instantiate(buttonPrefab, cardGrid.transform.position, buttonPrefab.transform.rotation);
        card.transform.SetParent(cardGrid.transform);
        info.buttonInstance = card;
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
        if (info.maxUpgradeReached)
        {
            upgradePanel.SetActive(false);
            _activeUpgradeCard = null;
            return;
        }
        
        upgradePanel.SetActive(true);
        _activeUpgradeCard = info;

        Debug.Log(info);
        
        upgradePanel.transform.Find("Header").GetComponent<TextMeshProUGUI>().text = info.upgradeName;
        upgradePanel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = info.upgradeDescription;
        
        var purchaseButton = upgradePanel.transform.Find("Purchase Button");
        var purchaseButtonOnClick = purchaseButton.GetComponent<Button>().onClick;
        
        purchaseButtonOnClick.RemoveAllListeners();
        purchaseButtonOnClick.AddListener(PurchaseButtonClicked);
        
        upgradePanel.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "C. " + info.upgradeCost;

        if (gameData.gold >= info.upgradeCost)
        {
            // Debug.Log(info.upgradeCost.ToString() + " > " + gameData.gold.ToString() + " : " + (info.upgradeCost > gameData.gold));
            purchaseButton.gameObject.SetActive(true); 
            purchaseButtonOnClick.AddListener(delegate { info.purchased.Invoke(info); });
            upgradePanel.transform.Find("Fund Warning").GetComponent<TextMeshProUGUI>().gameObject.SetActive(false);
        }
        else
        {
            // Debug.Log(info.upgradeCost.ToString() + " > " + gameData.gold.ToString() + " : " + (info.upgradeCost > gameData.gold));
            purchaseButton.gameObject.SetActive(false);
            upgradePanel.transform.Find("Fund Warning").GetComponent<TextMeshProUGUI>().gameObject.SetActive(true);
        }
    }

    public void HideUpgradeCard()
    {
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
