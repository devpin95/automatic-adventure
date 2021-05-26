using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Serialization;


public class RadarBlip
{
    public GameObject blipInstance;
    public GameObject trackedEnemy;
}

public class TowerRadarController : MonoBehaviour
{
    public LayerMask scannableLayers;
    public float displayRadius;
    [FormerlySerializedAs("radarRange")] public float radarRadius = 50f;

    public GameObject blipPrefab;

    private List<RadarBlip> _trackedEnemies = new List<RadarBlip>();
    private GameObject _radarAnchor;
    private float _rangeToDisplayRatio;
    // Start is called before the first frame update
    void Start()
    {
        _rangeToDisplayRatio = displayRadius / radarRadius;
        _radarAnchor = GameObject.Find("Radar Anchor");
        // Debug.Log("Radar Anchor at " + _radarAnchor.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_radarAnchor) return;
        
        CleanupTrackedEnemies();
        
        Collider[] enemies = Physics.OverlapSphere(_radarAnchor.transform.position, radarRadius, scannableLayers);
        for (int i = 0; i < enemies.Length; ++i)
        {
            if (!EnemyBeingTracked(enemies[i].gameObject))
            {
                StartTrackingEnemy(enemies[i].gameObject);
            }
        }

        UpdateRadarDisplay();
    }

    private void UpdateRadarDisplay()
    {
        foreach (var blip in _trackedEnemies)
        {
            // first, check if the enemy is still active
            if (!blip.trackedEnemy.activeInHierarchy)
            {
                StopTrackingEnemy(blip);
                continue;
            };
            
            // direction vector from the center of the radar to the enemy
            Vector3 positionVector = blip.trackedEnemy.transform.position - _radarAnchor.transform.position;
            // Debug.Log("Raw Position Vector: " + positionVector);
            
            float distanceFromCenter =
                Vector3.Distance(_radarAnchor.transform.position, blip.trackedEnemy.transform.position);
            
            float displayDistance = distanceFromCenter * _rangeToDisplayRatio;

            // set the vector to the correct length for the display
            // Debug.Log("Scaled Position Vector: " + positionVector.normalized * displayDistance);
            positionVector = positionVector.normalized * displayDistance;
            
            // Debug.Log("Position Vector: " + positionVector + " Raw Distance: " + distanceFromCenter + " Display Distance: " + displayDistance);

            blip.blipInstance.transform.position = transform.position + positionVector;
        }
    }

    private void CleanupTrackedEnemies()
    {
        List<RadarBlip> unactiveList = new List<RadarBlip>();

        // loop through and find all the unactive blips
        foreach (var blip in _trackedEnemies)
        {
            if ( !blip.trackedEnemy.activeInHierarchy || !WithinRange(blip.trackedEnemy) ) unactiveList.Add(blip);
        }

        // loop through the blips that we found were inactive and remove them from the tracked blips
        foreach (var unactiveBlip in unactiveList)
        {
            _trackedEnemies.Remove(unactiveBlip);
            Destroy(unactiveBlip.blipInstance);
        }
    }

    private bool EnemyBeingTracked(GameObject obj)
    {
        foreach (var blip in _trackedEnemies)
        {
            if (blip.trackedEnemy.Equals(obj)) return true;
        }

        return false;
    }

    private void StartTrackingEnemy(GameObject obj)
    {
        Debug.Log("TRACKING ENEMY");
        RadarBlip blip = new RadarBlip();
        blip.trackedEnemy = obj;
        blip.blipInstance = Instantiate(blipPrefab, transform.position, blipPrefab.transform.rotation);
        blip.blipInstance.transform.SetParent(transform);
        
        // make a new instance of the blip
        
        _trackedEnemies.Add(blip);
    }

    private void StopTrackingEnemy(RadarBlip blip)
    {
        _trackedEnemies.Remove(blip);
        Destroy(blip.blipInstance.gameObject);
    }

    private bool WithinRange(GameObject obj)
    {
        float distanceFromCenter =
            Vector3.Distance(_radarAnchor.transform.position, obj.transform.position);
        return distanceFromCenter <= radarRadius;
    }

    public void OnDrawGizmos()
    {
        if (!_radarAnchor) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_radarAnchor.transform.position, radarRadius);
    }
}
