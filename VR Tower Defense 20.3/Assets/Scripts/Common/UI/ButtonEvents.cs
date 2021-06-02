using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(BaseEventData eventData)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var tmp = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            if ( tmp ) tmp.color = Color.black;
        }
    }
    
    public void OnPointerExit(BaseEventData eventData)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var tmp = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            if ( tmp ) tmp.color = Color.white;
        }
    }
}
