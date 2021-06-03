using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeUIGameValueManager : MonoBehaviour
{
    public GameData gameData;
    public TextMeshProUGUI creditField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        creditField.text = "C. " + gameData.gold;
    }
}
