using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveOffsetCalibrationData : MonoBehaviour
{
    public SteamVR_TrackedObject tracker;
    private ZEDSteamVRControllerManager zedSteamVRControllerManager;
    private ZEDOffsetController zedOffsetController;

    void Start()
    {
        zedSteamVRControllerManager = GetComponent<ZEDSteamVRControllerManager>();
        zedOffsetController = GetComponent<ZEDOffsetController>();
        Invoke("saveCalibrationData", 10);
    }

    //-Todo: Also, check if there is no value at controllerIndexZEDHolder, if not show a screen that says "put on you VR headset"
    void saveCalibrationData()
    {
        zedSteamVRControllerManager.controllerIndexZEDHolder = (int)tracker.index;
        zedOffsetController.SaveZEDPos();
    }
}
