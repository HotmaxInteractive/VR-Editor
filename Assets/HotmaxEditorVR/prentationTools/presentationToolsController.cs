using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class presentationToolsController : MonoBehaviour
{
    private GreenScreenManager greenScreenManager;
    private Transform zedGreenScreenCamera;

    void Start()
    {
        greenScreenManager = FindObjectOfType<GreenScreenManager>();
        zedGreenScreenCamera = greenScreenManager.transform;
        transform.parent = zedGreenScreenCamera;
        transform.position = zedGreenScreenCamera.position;
        transform.localEulerAngles = new Vector3(0, 180, 0);
    }
}
