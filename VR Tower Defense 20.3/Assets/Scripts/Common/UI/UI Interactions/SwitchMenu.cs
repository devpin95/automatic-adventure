using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMenu : MonoBehaviour
{
    public GameObject parentMenu;
    public GameObject switchToMenu;

    public void Switch()
    {
        //hide parent menu
        parentMenu.SetActive(false);
        // show next menu
        switchToMenu.SetActive(true);
    }
}
