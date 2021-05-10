using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMonitorScreenController : MonoBehaviour
{
    public RenderTexture wallCamera;
    public RenderTexture spawnCamera;

    public CanvasRenderer  shaderRenderer;

    public string renderTextureId; // Texture2D_d49e89ebd81942eda87e771185995ba2

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraSwitchFlipped(bool state)
    {
        if (state)
        {
            shaderRenderer.GetMaterial().SetTexture(renderTextureId, wallCamera);
        }
        else
        {
            shaderRenderer.GetMaterial().SetTexture(renderTextureId, spawnCamera);
        }
    }
}
