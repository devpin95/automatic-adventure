using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BountyNotificationManager : MonoBehaviour
{
    public ActiveBounties activeBounties;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveNotification(BountyTrackedData data)
    {
        // Debug.Log("Bounty Type: " + data.bountyType + " + " + data.trackedDataId);
        List<Bounty> targetBounties = activeBounties.bounties.Where(t => t.bountyType == data.bountyType && t.bountyActionId == data.trackedDataId).ToList();

        foreach (var bounty in targetBounties)
        {
            if (bounty.trackerType == Bounty.TrackerType.PredicatedAccumulator) bounty.rhs = data.testVal;
            
            bounty.Evaluate();
            
            Debug.Log("(" + bounty.accumulator + "/" + bounty.completionVal + ") " + bounty.bountyName + " - " + bounty.description);
        }
    }
}
