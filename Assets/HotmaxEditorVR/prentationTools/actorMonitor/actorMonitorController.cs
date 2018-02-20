using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actorMonitorController : MonoBehaviour
{
    //TODO: Finds camera_left, populates it with actorMonitor.cs, and makes this GameObject the preview target
    private GreenScreenManager greenScreenManager;
    private GameObject zedGreenScreenCamera;

    void Awake ()
    {
        greenScreenManager = FindObjectOfType<GreenScreenManager>();
        zedGreenScreenCamera = greenScreenManager.gameObject;
        zedGreenScreenCamera.AddComponent<actorMonitor>();
        zedGreenScreenCamera.GetComponent<actorMonitor>().target = gameObject;
    }
}
