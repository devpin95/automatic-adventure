using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeapon : MonoBehaviour
{
    public RenderTexture renderTexture;

    public GameObject gui;
    
    private Camera activeCamera;
    private Transform attachPoint;
    private RenderTexture cameraFeed;

    public RenderTexture CameraFeed => cameraFeed;
    public Camera ActiveCamera => activeCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        activeCamera = transform.Find("Camera Feed").GetComponent<Camera>();
        if (!activeCamera) throw new Exception("Heavy Weapon needs a camera object");
        
        cameraFeed = activeCamera.targetTexture;
        if (!cameraFeed) throw new Exception("Heavy Weapon needs to render camera view to texture");

        attachPoint = transform.Find("Attach Point").GetComponent<Transform>();
        if (!attachPoint) throw new Exception("Heavy Weapon needs an attach point");
    }

    public Vector3 AttachPoint()
    {
        return transform.Find("Attach Point").transform.position;
    }
}
