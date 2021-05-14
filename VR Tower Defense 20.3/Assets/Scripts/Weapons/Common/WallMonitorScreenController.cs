using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WallMonitorScreenController : MonoBehaviour
{
    [FormerlySerializedAs("cameraShadedObject")] public GameObject cameraView;
    [FormerlySerializedAs("availableCameras")] public RenderTexture[] availableCameraRenderTextures;
    private int cameraMonitorIndex = 0;

    public CanvasRenderer shaderRenderer;

    public string renderTextureId; // Texture2D_d49e89ebd81942eda87e771185995ba2
    private bool cameraState = true;

    public void CameraState(bool state)
    {
        if ( state ) cameraView.SetActive(true);
        else cameraView.SetActive(false);

        cameraState = state;
    }
    
    public void ChangeMonitorActiveCamera()
    {
        if (!cameraView.activeInHierarchy) return;
        
        ++cameraMonitorIndex;

        if (cameraMonitorIndex >= availableCameraRenderTextures.Length) cameraMonitorIndex = 0;
        
        shaderRenderer.GetMaterial().SetTexture(renderTextureId, availableCameraRenderTextures[cameraMonitorIndex]);
    }
}
