using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virtualCameraInit : MonoBehaviour
{
    public SteamVR_TrackedObject tracker;
    private ZEDSteamVRControllerManager zedSteamVRControllerManager;
    private ZEDOffsetController zedOffsetController;

    void Start()
    {
        zedSteamVRControllerManager = FindObjectOfType<ZEDSteamVRControllerManager>();
        zedOffsetController = zedSteamVRControllerManager.GetComponent<ZEDOffsetController>();
        Invoke("setTrackerDeviceIndex", 5);
    }

    //-Todo: Also, check if there is no value at controllerIndexZEDHolder, if not show a screen that says "turn on controllers and tracker"
    //--Sets the virtual camera for the zed at the position of the tracker
    void setTrackerDeviceIndex()
    {
        //--sets the virtual camera to the tracker
        zedSteamVRControllerManager.controllerIndexZEDHolder = (int)tracker.index;
        //--saves the initial offset and the tracker index config
        zedOffsetController.SaveZEDPos();
    }
}
