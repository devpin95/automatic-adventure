using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidStintUISoundController : MonoBehaviour
{
    public AudioClip buttonClickSound;
    public AudioClip purchaseSound;
    public AudioClip startNextStintSound;

    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayUISound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void PlayButtonClickSound()
    {
        PlayUISound(buttonClickSound);
    }

    public void PlayPurchaseSound()
    {
        PlayUISound(purchaseSound);
    }
    
    public void PlayNextStintSound()
    {
        PlayUISound(startNextStintSound);
    }
}
