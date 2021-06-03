using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButtonClicked : MonoBehaviour
{
    public UpgradeCard upgradeCardInfo;
    public CEvent_UpgradeCard upgradeButtonClickedEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnButtonClicked()
    {
        upgradeButtonClickedEvent.Raise(upgradeCardInfo);
    }

    // public void OnButtonDeselected()
    // {
    //     upgradeButtonDeselected.Raise();
    // }
}
