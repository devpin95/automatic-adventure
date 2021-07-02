using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScrollAreaController : MonoBehaviour
{
    public GameObject gridArea;
    public GameObject emptyListIndicator;
    public Button paginatorButtonUp;
    public Button paginatorButtonDown;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int childCount = ActiveChildCount();
        
        if (childCount == 0)
        {
            emptyListIndicator.SetActive(true);
            paginatorButtonDown.interactable = false;
            paginatorButtonUp.interactable = false;
        }
        else
        {
            emptyListIndicator.SetActive(false);
            paginatorButtonDown.interactable = true;
            paginatorButtonUp.interactable = true;
        }
    }

    private int ActiveChildCount()
    {
        int count = 0;
        
        for (int i = 0; i < gridArea.transform.childCount; ++i)
        {
            if (gridArea.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                ++count;
            }
        }

        return count;
    }
}
