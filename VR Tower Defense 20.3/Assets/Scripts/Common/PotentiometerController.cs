using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PotentiometerController : MonoBehaviour
{
    public Transform lowVal;
    public Transform highVal;

    public CEvent_Float potValueChange;
    
    private Vector3 track;
    private float distance;

    private bool selected = false;
    private XRBaseInteractor currentInteractor;

    private float zoom = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.Lerp(lowVal.position, highVal.position, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            Vector3 low = lowVal.position;
            Vector3 high = highVal.position;
        
            track = high - low; // a vector from the low position to the high position
            distance = Vector3.Distance(high, low);
            
            Vector3 lowToHand = currentInteractor.transform.position - low;
            float dotproduct = Vector3.Dot(lowToHand, track); // project the hand vector onto the track vector
            float dotmag = dotproduct / track.magnitude;

            float lerp = dotmag / distance;
            transform.position = Vector3.Lerp(low, high, lerp);

            zoom = Mathf.Clamp01(lerp);
            
            potValueChange.Raise(zoom);
            
            // Debug.Log(zoom);

            // Debug.Log("DISTANCE:" + distance + " DOTMAG:" + dotmag + " LERP:" + lerp);
            //
            // Debug.DrawLine(low, high);
            // Debug.DrawLine(low, currentInteractor.transform.position);
        }
    }

    public void OnSelectEntered(XRBaseInteractor interactor)
    {
        selected = true;
        currentInteractor = interactor;
    }

    public void OnSelectExited(XRBaseInteractor interactor)
    {
        selected = false;
        currentInteractor = null;
    }
}
