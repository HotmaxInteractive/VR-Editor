using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--NOTE: This script needs to be placed on the ZED Camera_Left GameObject
public class renderToActorMonitor : MonoBehaviour
{
    public GameObject actorMonitor;
    private RenderTexture preview;
    private Material mat;

    void Start()
    {
        preview = new RenderTexture(1024, 1024, 24);
        mat = actorMonitor.GetComponent<Renderer>().material;
    }

    void Update()
    {
        mat.SetTexture("_MainTex", preview);       
    }

    void OnApplicationQuit()
    {
        preview.Release();       
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest);
        Graphics.Blit(src, preview);      
    }
}
