using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainHudBountyUIController : MonoBehaviour
{
    public ActiveBounties activeBounties;
    private List<Bounty> _sortedBounties;

    [Header("Bounty Containers")]
    public GameObject bounty1;
    public GameObject bounty2;
    public GameObject bounty3;
    
    [Header("Bounty Name TMPs")]
    public TextMeshProUGUI bounty1Name;
    public TextMeshProUGUI bounty2Name;
    public TextMeshProUGUI bounty3Name;
    
    [Header("Bounty Descriptions TMPs")]
    public TextMeshProUGUI bounty1Description;
    public TextMeshProUGUI bounty2Description;
    public TextMeshProUGUI bounty3Description;

    [Header("Bounty Icon Sprites")]
    public Image bounty1Icon;
    public Image bounty2Icon;
    public Image bounty3Icon;

    private List<GameObject> _bountyContainers = new List<GameObject>();
    private List<Image> _bountyImages = new List<Image>();
    private List<TextMeshProUGUI> _bountyDescriptions = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _bountyNames = new List<TextMeshProUGUI>();

    private bool bountiesAreActive = false;
    
    // Start is called before the first frame update

    // Update is called once per frame
    void Start()
    {
        _sortedBounties = activeBounties.bounties.OrderBy(t => t.bountyType).ToList();
        
        if (_sortedBounties.Count > 0) bountiesAreActive = true;
        
        _bountyContainers.Add(bounty1);
        _bountyContainers.Add(bounty2);
        _bountyContainers.Add(bounty3);
        
        _bountyImages.Add(bounty1Icon);
        _bountyImages.Add(bounty2Icon);
        _bountyImages.Add(bounty3Icon);
        
        _bountyNames.Add(bounty1Name);
        _bountyNames.Add(bounty2Name);
        _bountyNames.Add(bounty3Name);
        
        _bountyDescriptions.Add(bounty1Description);
        _bountyDescriptions.Add(bounty2Description);
        _bountyDescriptions.Add(bounty3Description);
        
        InitBounties();
        // bounty1Icon.sprite = _sortedBounties[0].iconDefault;
        // bounty2Icon.sprite = _sortedBounties[1].iconDefault;
        // bounty3Icon.sprite = _sortedBounties[2].iconDefault;
        
        // bounty1Description.text = _sortedBounties[0].shortDescription;
        // bounty2Description.text = _sortedBounties[1].shortDescription;
        // bounty3Description.text = _sortedBounties[2].shortDescription;
    }

    void Update()
    {
        if (!bountiesAreActive) return;

        for (int i = 0; i < _sortedBounties.Count; ++i)
        {
            if (_sortedBounties[i].completed) SetTextToCompleted(_sortedBounties[i], _bountyNames[i], _bountyDescriptions[i], _bountyImages[i]);
            else SetText(_sortedBounties[i], _bountyNames[i]);
        }

        // if (_sortedBounties[0].completed) SetTextToCompleted(_sortedBounties[0], bounty1Name, bounty1Description, bounty1Icon);
        // else SetText(_sortedBounties[0], bounty1Name);
        //
        // if (_sortedBounties[1].completed) SetTextToCompleted(_sortedBounties[1], bounty2Name, bounty2Description, bounty2Icon);
        // else SetText(activeBounties.bounties[1], bounty2Name);
        //
        // if (_sortedBounties[2].completed) SetTextToCompleted(_sortedBounties[2], bounty3Name, bounty3Description, bounty3Icon);
        // else SetText(_sortedBounties[2], bounty3Name);
    }

    private void SetTextToCompleted(Bounty bounty, TextMeshProUGUI field, TextMeshProUGUI desc,  Image icon)
    {
        field.text = bounty.bountyName;
        field.fontStyle = FontStyles.Strikethrough;
        field.color = new Color(255, 255, 255, 0.1f);
        desc.text = "";
        icon.color = new Color(255, 255, 255, 0.1f);
    }

    private void SetText(Bounty bounty, TextMeshProUGUI field)
    {
        field.text = bounty.bountyName + " (" + bounty.accumulator + "/" +
                     bounty.completionVal + ")";
    }
    
    private void InitIcons()
    {
        for (int i = 0; i < _sortedBounties.Count; ++i)
        {
            _bountyImages[i].sprite = _sortedBounties[i].iconDefault;
        }
    }

    private void InitBounties()
    {
        for (int i = 0; i < _sortedBounties.Count; ++i)
        {
            _bountyContainers[i].SetActive(true);
            _bountyImages[i].sprite = _sortedBounties[i].iconDefault;
            _bountyDescriptions[i].text = _sortedBounties[i].shortDescription;
            _bountyNames[i].text = _sortedBounties[i].bountyName;
        }
    }
}
