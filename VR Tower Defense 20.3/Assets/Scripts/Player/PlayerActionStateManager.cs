using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionStateManager : MonoBehaviour
{
    public enum ActionMap
    {
        Freemovement,
        Machinegun,
        HeavyWeaponSniperRifle
    }

    public Dictionary<ActionMap, string> ActionMapNames = new Dictionary<ActionMap, string> 
    {
        {ActionMap.Freemovement, "Free Movement"},
        {ActionMap.Machinegun, "Machinegun"},
        {ActionMap.HeavyWeaponSniperRifle, "Sniper Rifle"}
    };
    
    public InputActionAsset inputActions;

    private PlayerInput _playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        SwitchCurrentActionMap((int)ActionMap.Freemovement);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCurrentActionMap(int map)
    {
        ActionMap mapt = (ActionMap) map;
        string mapname = ActionMapNames[mapt];
        
        Debug.Log("Switching action map to " + mapname);

        _playerInput.currentActionMap.Disable();
        _playerInput.currentActionMap = inputActions.FindActionMap(mapname);
        _playerInput.currentActionMap.Enable();
    }
}
