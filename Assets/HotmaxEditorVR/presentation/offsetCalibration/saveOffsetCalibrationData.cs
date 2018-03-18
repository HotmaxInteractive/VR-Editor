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
        zedSteamVRControllerManager = FindObjectOfType<ZEDSteamVRControllerManager>();
        zedOffsetController = zedSteamVRControllerManager.GetComponent<ZEDOffsetController>();
        Invoke("setTrackerDeviceIndex", 10);
    }

    //-Todo: Also, check if there is no value at controllerIndexZEDHolder, if not show a screen that says "turn on controllers and tracker"
    void setTrackerDeviceIndex()
    {
        zedSteamVRControllerManager.controllerIndexZEDHolder = (int)tracker.index;
        zedOffsetController.SaveZEDPos();
    }
}
