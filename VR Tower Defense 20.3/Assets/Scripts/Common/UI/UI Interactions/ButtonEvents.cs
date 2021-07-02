using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ButtonEvents : MonoBehaviour
{
    public Color unselectedTextColor;
    public Color selectedTextColor;
    public CEvent upgradeButtonDeselected;

    public List<TextMeshProUGUI> exclusionList = new List<TextMeshProUGUI>();

    private bool _selected = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(BaseEventData eventData = null)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var tmp = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            if ( tmp && !exclusionList.Contains(tmp) ) tmp.color = selectedTextColor;
        }
    }
    
    public void OnPointerExit(BaseEventData eventData = null)
    {
        if (_selected) return;
        
        for (int i = 0; i < transform.childCount; ++i)
        {
            var tmp = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            if ( tmp && !exclusionList.Contains(tmp) ) tmp.color = unselectedTextColor;
        }
    }

    public void OnButtonClicked()
    {
        _selected = true;
    }

    public void OnButtonSelected(BaseEventData eventData)
    {
        Debug.Log("Button selected!");
        _selected = true;
        OnPointerEnter(eventData);
    }

    public void OnButtonDeselected(BaseEventData eventData)
    {
        Debug.Log("Button DEEEEselected!");
        _selected = false;
        OnPointerExit(eventData);
        upgradeButtonDeselected?.Raise();
    }

    public void DeselectButton()
    {
        _selected = false;
        OnPointerExit();
    }
}
