using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BountySelectionController : MonoBehaviour
{
    public BountyList bountyList;
    public ActiveBounties activeBounties;
    public GameObject bountyButtonPrefab;
    [FormerlySerializedAs("bountyCanvasGroup")] public GameObject bountyGroup;
    private CanvasGroup _bountyCanvasGroup;
    
    [Header("Kill Bounty Group")]
    public GameObject killBountyGrid;
    public TextMeshProUGUI killBountyLabel;
    public TextMeshProUGUI killBountyCount;
    private List<Bounty> _killBounties = new List<Bounty>();
    
    [Header("Action Bounty Group")]
    public GameObject actionBountyGrid;
    public TextMeshProUGUI actionBountyLabel;
    public TextMeshProUGUI actionBountyCount;
    private List<Bounty> _actionBounties = new List<Bounty>();
    
    [Header("Wave Bounty Group")]
    public GameObject waveBountyGrid;
    public TextMeshProUGUI waveBountyLabel;
    public TextMeshProUGUI waveBountyCount;
    private List<Bounty> _waveBounties = new List<Bounty>();

    [Header("Panel")] 
    public GameObject panel;
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI descriptionField;
    public TextMeshProUGUI rewardField;
    public TextMeshProUGUI bountyTypeFieldText;
    public Image bountyTypeFieldImage;
    public TextMeshProUGUI availabilityField;
    public TextMeshProUGUI completionField;
    [Space(11)] 
    public GameObject progressGroup;
    public TextMeshProUGUI progressField;
    [Space(11)]
    public GameObject receiveButton;
    public GameObject abandonButton;

    [Header("Active Bounties")] 
    public TextMeshProUGUI activeBountiesHeader;
    public GameObject activeBountiesGrid;
    
    [FormerlySerializedAs("promptText")] [Header("Prompt")]
    public TextMeshProUGUI promptHeader;
    public TextMeshProUGUI promptText;

    [Header("Prompt Texts")] 
    public string fullBountyListHeader;
    public string fullBountyListText;
    [Space(11)]
    public string defaultBountyListHeader;
    public string defaultBountyListText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _bountyCanvasGroup = bountyGroup.GetComponent<CanvasGroup>();

        CheckBountyListStatus();

        PopulateActiveBountiesGrid();
        activeBountiesHeader.text = "Active Bounties (" + activeBounties.bounties.Count + "/" +
                                    activeBounties.maxActiveBounties + ")";
        
        // init kill bounties
        _killBounties = bountyList.bounties
            .FindAll(t => t.bountyType == Bounty.BountyTypes.Kill 
                          && !activeBounties.bounties.Contains(t))
            .OrderByDescending(t=> t.reward)
            .ToList();
        
        killBountyCount.text = _killBounties.Count.ToString();
        PopulateGrid(_killBounties, killBountyGrid);

        // init action bounties
        _actionBounties = bountyList.bounties
            .FindAll(t => t.bountyType == Bounty.BountyTypes.Action && 
                          !activeBounties.bounties.Contains(t))
            .OrderByDescending(t=> t.reward)
            .ToList();
        
        actionBountyCount.text = _actionBounties.Count.ToString();
        PopulateGrid(_actionBounties, actionBountyGrid);

        // init wave bounties
        _waveBounties = bountyList.bounties
            .FindAll(t => t.bountyType == Bounty.BountyTypes.Wave 
                          && !activeBounties.bounties.Contains(t))
            .OrderByDescending(t=> t.reward)
            .ToList();
        
        waveBountyCount.text = _waveBounties.Count.ToString();
        PopulateGrid(_waveBounties, waveBountyGrid);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PopulateGrid(List<Bounty> bounties, GameObject grid)
    {
        foreach (var bounty in bounties)
        {
            var buttonInstance = InstantiateBountyButton(bounty, grid);
            // buttonInstance.transform.SetParent(grid.transform);
            var btnComp = buttonInstance.GetComponent<Button>();

            var capturedBounty = bounty;
            btnComp.onClick.AddListener(() => ShowBountyPanel(capturedBounty, buttonInstance));
        }
    }

    private void PopulateActiveBountiesGrid()
    {
        foreach (var bounty in activeBounties.bounties)
        {
            CreateActiveBountyButton(bounty);
        }
    }

    private void ShowBountyPanel(Bounty bounty, GameObject buttonInstance)
    {
        Debug.Log("Showing panel for " + bounty.bountyName);
        
        panel.SetActive(true);
        receiveButton.SetActive(true);
        abandonButton.SetActive(false);
        progressGroup.gameObject.SetActive(false);

        headerField.text = bounty.bountyName;
        descriptionField.text = bounty.description;
        rewardField.text = "C. " + bounty.reward.ToString("n0");
        bountyTypeFieldText.text = "(" + bounty.bountyTypeName[bounty.bountyType] + ")";
        bountyTypeFieldImage.sprite = bounty.iconSelected;
        availabilityField.text = "Always I guess";
        completionField.text = 0.ToString();
        
        receiveButton.GetComponent<Button>().onClick.RemoveAllListeners();
        receiveButton.GetComponent<Button>().onClick.AddListener(() => ReceiveBounty(bounty, buttonInstance));
    }
    
    private void HideBountyPanel()
    {
        CheckBountyListStatus();
        
        receiveButton.GetComponent<ButtonEvents>().DeselectButton();
        abandonButton.GetComponent<ButtonEvents>().DeselectButton();
        
        panel.SetActive(false);
    }

    private void ShowBountyPanelForActiveBounty(Bounty bounty, GameObject buttonInstance)
    {
        panel.SetActive(true);
        
        receiveButton.SetActive(false);
        abandonButton.SetActive(true);
        progressGroup.gameObject.SetActive(true);
        
        headerField.text = bounty.bountyName;
        descriptionField.text = bounty.description;
        rewardField.text = "C. " + bounty.reward.ToString("n0");
        bountyTypeFieldText.text = "(" + bounty.bountyTypeName[bounty.bountyType] + ")";
        bountyTypeFieldImage.sprite = bounty.iconSelected;
        availabilityField.text = "Always I guess";
        completionField.text = 0.ToString();

        if (bounty.completionVal == 0)
        {
            progressField.text = "--";
        }
        else
        {
            progressField.text = "%" + (bounty.accumulator / bounty.completionVal).ToString("n0");
        }

        abandonButton.GetComponent<Button>().onClick.RemoveAllListeners();
        abandonButton.GetComponent<Button>().onClick.AddListener(() => AbandonBounty(bounty, buttonInstance));
    }

    private void ReceiveBounty(Bounty bounty, GameObject buttonInstance)
    {
        activeBounties.AddBounty(bounty);
        CreateActiveBountyButton(bounty);
        Destroy(buttonInstance);

        CheckBountyListStatus();

        activeBountiesHeader.text = "Active Bounties (" + activeBounties.bounties.Count + "/" +
                               activeBounties.maxActiveBounties + ")";

        HideBountyPanel();
        UpdateBountyLists(bounty, true);
    }

    private void AbandonBounty(Bounty bounty, GameObject buttonInstance)
    {
        activeBounties.RemoveBounty(bounty);
        Destroy(buttonInstance);
        HideBountyPanel();
        
        activeBountiesHeader.text = "Active Bounties (" + activeBounties.bounties.Count + "/" +
                                    activeBounties.maxActiveBounties + ")";

        UpdateBountyLists(bounty, false);
    }

    private GameObject InstantiateBountyButton(Bounty bounty, GameObject grid)
    {
        GameObject bountyButton = Instantiate(bountyButtonPrefab, grid.transform);
        bountyButton.GetComponent<Image>().sprite = bounty.buttonDefault;
        var btnComp = bountyButton.GetComponent<Button>();

        SpriteState ss = new SpriteState();
        ss.highlightedSprite = bounty.buttonSelected;
        ss.pressedSprite = bounty.buttonSelected;
        ss.selectedSprite = bounty.buttonSelected;
            
        btnComp.spriteState = ss;

        return bountyButton;
    }

    private void CreateActiveBountyButton(Bounty bounty)
    {
        var buttonInstance = InstantiateBountyButton(bounty, activeBountiesGrid);
        // buttonInstance.transform.SetParent(activeBountiesGrid.transform);
        var btnComp = buttonInstance.GetComponent<Button>();

        var capturedBounty = bounty;
        btnComp.onClick.AddListener(() => ShowBountyPanelForActiveBounty(capturedBounty, buttonInstance));
    }

    private void UpdatePrompts(string header, string des)
    {
        promptHeader.text = header;
        promptText.text = des;
    }

    private void CheckBountyListStatus()
    {
        if (activeBounties.bounties.Count >= activeBounties.maxActiveBounties)
        {
            UpdateBountyListStatus(0.1f, false);
            UpdatePrompts(fullBountyListHeader, fullBountyListText);
        }
        else
        {
            UpdateBountyListStatus(1, true);
            UpdatePrompts(defaultBountyListHeader, defaultBountyListText);
        }
    }
    private void UpdateBountyListStatus(float alpha, bool interactable)
    {
        _bountyCanvasGroup.alpha = alpha;
        _bountyCanvasGroup.interactable = interactable;
    }

    private void UpdateBountyLists(Bounty bounty, bool deletion)
    {
        if (bounty.bountyType == Bounty.BountyTypes.Action)
        {
            // update action bounty list
            _actionBounties = bountyList.bounties
                .FindAll(t => t.bountyType == Bounty.BountyTypes.Action && 
                              !activeBounties.bounties.Contains(t))
                .OrderByDescending(t=> t.reward)
                .ToList();
        
            actionBountyCount.text = _actionBounties.Count.ToString();

            if (!deletion)
            {
                // remove all bounties from the list
                RemoveAllButtonsFromGrid(actionBountyGrid);
                PopulateGrid(_actionBounties, actionBountyGrid);
            }
        } 
        
        else if (bounty.bountyType == Bounty.BountyTypes.Kill)
        {
            _killBounties = bountyList.bounties
                .FindAll(t => t.bountyType == Bounty.BountyTypes.Kill 
                              && !activeBounties.bounties.Contains(t))
                .OrderByDescending(t=> t.reward)
                .ToList();
            
            killBountyCount.text = _killBounties.Count.ToString();
            
            if (!deletion)
            {
                // remove all bounties from the list
                RemoveAllButtonsFromGrid(killBountyGrid);
                PopulateGrid(_killBounties, killBountyGrid);
            }
        } 
        
        else if (bounty.bountyType == Bounty.BountyTypes.Wave)
        {
            // update wave list
            _waveBounties = bountyList.bounties
                .FindAll(t => t.bountyType == Bounty.BountyTypes.Wave 
                              && !activeBounties.bounties.Contains(t))
                .OrderByDescending(t=> t.reward)
                .ToList();
        
            waveBountyCount.text = _waveBounties.Count.ToString();
            
            if (!deletion)
            {
                // remove all bounties from the list
                RemoveAllButtonsFromGrid(waveBountyGrid);
                PopulateGrid(_waveBounties, waveBountyGrid);
            }
        }
    }

    private void RemoveAllButtonsFromGrid(GameObject grid)
    {
        for (int i = 0; i < grid.transform.childCount; ++i)
        {
            Destroy(grid.transform.GetChild(i).gameObject);
        }
    }
}
