using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIGameValueManager : MonoBehaviour
{
    public GameData gameData;
    public TextMeshProUGUI creditField;

    private EventSystem _eventSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        creditField.text = "C. " + gameData.gold.ToString("n0");
    }

    public void DeselectAllButtons()
    {
        
    }
}
