using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseOverlayMenu : MonoBehaviour
{
    public GameObject parentOverlayMenu;

    public void CloseOverlay()
    {
        parentOverlayMenu.SetActive(false);
    }
}
