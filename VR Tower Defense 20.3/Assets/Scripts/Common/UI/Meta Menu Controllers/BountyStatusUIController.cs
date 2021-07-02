using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BountyStatusUIController : MonoBehaviour
{
    public GameObject bountyStatusButtonPrefab;
    public ActiveBounties activeBounties;
    public GameData gameData;
    public GameObject bountyStatusGrid;
    public GameObject emptyListIndicator;
    
    // Start is called before the first frame update
    void Start()
    {
        emptyListIndicator.gameObject.SetActive(true);
        
        foreach (var bounty in activeBounties.bounties)
        {
            Debug.Log("Making button for " + bounty.bountyName );
            GameObject statusButton = Instantiate(bountyStatusButtonPrefab, transform.position, bountyStatusButtonPrefab.transform.rotation);
            statusButton.transform.SetParent(bountyStatusGrid.transform);
            statusButton.transform.Find("Bounty Name").GetComponent<TextMeshProUGUI>().text = bounty.bountyName;
            statusButton.transform.Find("Bounty Icon").GetComponent<Image>().sprite = bounty.iconDefault;
            statusButton.transform.Find("Bounty Description").GetComponent<TextMeshProUGUI>().text = bounty.description;

            if (bounty.completed)
            {
                statusButton.transform.Find("Status")
                    .transform.Find("Receive").gameObject.SetActive(true);
                
                statusButton.transform.Find("Status")
                    .transform.Find("Receive").transform.Find("Reward")
                    .GetComponent<TextMeshProUGUI>().text = "C." + bounty.reward.ToString("n0");

                int capturedReward = bounty.reward;
                
                AddListener(statusButton.transform.Find("Status")
                    .transform.Find("Receive")
                    .transform.Find("Receive Button").GetComponent<Button>(), statusButton, bounty);
                
                // statusButton.transform.Find("Status")
                //     .transform.Find("Receive")
                //     .transform.Find("Receive Button").GetComponent<Button>()
                //     .onClick.AddListener(() => ReceiveBountyReward(capturedReward) );
            }
            else
            {
                statusButton.transform.Find("Status")
                    .transform.Find("Pending Text").gameObject.SetActive(true);
            }
            
            emptyListIndicator.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void ReceiveBountyReward(GameObject instance, int reward)
    // {
    //     Debug.Log("Accepting " + reward + " credit reward");
    //     Destroy(instance);
    //
    //     if (bountyStatusGrid.transform.childCount == 0)
    //     {
    //         emptyListIndicator.gameObject.SetActive(true);
    //     }
    // }

    private void AddListener(Button button, GameObject instance, Bounty bounty)
    {

        button.onClick.AddListener(() =>
        {
            Debug.Log("Accepting " + bounty.reward + " credit  from " + bounty.bountyName);
            gameData.gold += bounty.reward;

            activeBounties.RemoveBounty(bounty);
            
            Destroy(instance);

            Debug.Log("bountyStatusGrid.transform.childCount = " + bountyStatusGrid.transform.childCount);

            if (bountyStatusGrid.transform.childCount == 2)
            {
                emptyListIndicator.gameObject.SetActive(true);
            }
        });
    }
}
