using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Heavy Weapons/List")]
public class HeavyWeaponList : ScriptableObject
{
    public List<HeavyWeaponCard> heavyWeaponsCard = new List<HeavyWeaponCard>();
    // private List<HeavyWeaponCard> excludedWeaponCards = new List<HeavyWeaponCard>();

    public void PurchaseItem(HeavyWeaponCard card)
    {
        if (heavyWeaponsCard.Contains(card))
        {
            card.purchased = true;
        }
    }

    public List<HeavyWeaponCard> GetActiveCardList()
    {
        var list = heavyWeaponsCard.Where(t => !t.purchased).ToList();

        return list.Count == 0 ? null : list;
    }

    public void ResetObject()
    {
        foreach (var card in heavyWeaponsCard)
        {
            card.purchased = false;
        }
    }
}
