using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RenderPreviewToPNG : MonoBehaviour
{
    [Header("File Info")] 
    public string cfileName;
    public string path;
    public bool prependAppDataPath = true;
    
    public int resWidth = 500;
    public int resHeight = 500;

    private Camera _camera;
    private bool _captureTaken = false;
    private string _fullPath;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _fullPath = (prependAppDataPath ? Application.dataPath : "") + path + (path.EndsWith("/") ? "" : "/") + cfileName + "_RAW.png";
        Debug.Log("Writing file to " + _fullPath);
    }

    private void LateUpdate()
    {
        // https://answers.unity.com/questions/22954/how-to-save-a-picture-take-screenshot-from-a-camer.html
        if (!_captureTaken)
        {
            _captureTaken = true;
    
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            _camera.targetTexture = rt;
    
            Texture2D screenshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            
            _camera.Render();
            RenderTexture.active = rt;
            screenshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            _camera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);
    
            byte[] bytes = screenshot.EncodeToPNG();
            
            System.IO.File.WriteAllBytes(_fullPath, bytes);
            
            Debug.Log("Image Captured...");
            Debug.Log("Stopping Application...");

            EditorApplication.isPlaying = false;
        }
    }

    // private string GenerateFileName()
    // {
    //     return string.Format("{0}{1}_{2}",
    //         (prependAppDataPath ? Application.dataPath + "_" : ""),
    //         System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    // }
}
