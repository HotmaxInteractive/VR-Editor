using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actorMonitor : MonoBehaviour
{
    private RenderTexture preview;

    [HideInInspector]
    public GameObject target;
    private Material mat;

    void Start()
    {
        preview = new RenderTexture(1024, 1024, 24);
    }

    void Update()
    {
        if (target != null && mat == null)
        {
            mat = target.GetComponent<Renderer>().material;
        }

        if (mat != null)
        {
            mat.SetTexture("_MainTex", preview);
        }
    }

    void OnApplicationQuit()
    {
        if (preview != null)
        {
            preview.Release();
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest);
        if (preview != null && target != null)
        {
            Graphics.Blit(src, preview);
        }
    }
}
