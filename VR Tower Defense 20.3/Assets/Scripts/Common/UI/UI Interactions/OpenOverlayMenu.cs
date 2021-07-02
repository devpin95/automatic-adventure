using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOverlayMenu : MonoBehaviour
{
    public GameObject overlayToOpen;

    public void OpenMenu()
    {
        overlayToOpen.SetActive(true);
    }
}
