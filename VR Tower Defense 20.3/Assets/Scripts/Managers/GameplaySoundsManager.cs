using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySoundsManager : MonoBehaviour
{
    public AudioSource killSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnKillHandler(int val, bool s = true)
    {
        if (val != 0) killSound.Play();
    }
}
