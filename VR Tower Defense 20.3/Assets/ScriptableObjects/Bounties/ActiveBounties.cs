using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Bounties/Active Bounties")]
public class ActiveBounties : ScriptableObject
{
    public int maxActiveBounties;
    public GameData gameData;
    public List<Bounty> bounties;

    public bool RemoveBounty(Bounty bounty)
    {
        bounty.ResetBounty();
        gameData.bountyCredits++;
        return bounties.Remove(bounty);
    }

    public void AddBounty(Bounty bounty)
    {
        if (bounties.Count == maxActiveBounties) return;
        bounties.Add(bounty);
        gameData.bountyCredits--;
    }
}
