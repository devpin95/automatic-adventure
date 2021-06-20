using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBountyMonitor : MonoBehaviour
{
    public CEvent_BountyTrackedData bountyNotification;
    private BountyTrackedData _bountyData = new BountyTrackedData();

    public GameData gameData;

    private bool _firstWave = true;
    
    // wall health
    private float _waveStartingWallHealth;
    private float _waveEndingWallHealth;
    
    // bounty IDs
    private string _wallHealthBountyID = "Wave Wall Health";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WaveStartHandler(int wave)
    {
        if (_firstWave)
        {
            _waveStartingWallHealth = gameData.wallCurrentHealth;
            _firstWave = false;
        }
        else
        {
            _waveEndingWallHealth = gameData.wallCurrentHealth;
            float healthDif = _waveStartingWallHealth - _waveEndingWallHealth;

            _bountyData.bountyType = Bounty.BountyTypes.Wave;
            _bountyData.testVal = healthDif;
            _bountyData.trackedDataId = _wallHealthBountyID;
            
            Debug.Log("Sending Wave Health Bounty Notification with Health Dif " + healthDif);

            _waveStartingWallHealth = gameData.wallCurrentHealth;
        }
    }

    public void StintEndHandler()
    {
        WaveStartHandler(0);
    }
}
