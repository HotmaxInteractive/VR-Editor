using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fastCalibrationSetup : MonoBehaviour
{
    public Transform Zed_Greenscreen;

    private SteamVR_TrackedController _trackedController2;

    private Vector3 initialControllerPosition;
    private Vector3 initialControllerRotation;

    public ZEDPointCloudManager pointCloudManager;

    public Transform calibrationCube;

    private void Start()
    {
        _trackedController2 = inputManager.trackedController2;
        _trackedController2.TriggerClicked += triggerClicked;
        _trackedController2.TriggerUnclicked += triggerUnclicked;
    }

    void OnApplicationQuit()
    {
        _trackedController2.TriggerClicked -= triggerClicked;
        _trackedController2.TriggerUnclicked -= triggerUnclicked;
    }
   
    void triggerClicked(object sender, ClickedEventArgs e)
    {
        //save position and rotation on initial click
        //...move controller...
        //on second click apply the difference in before and after to Zed Greenscreen
        if (Vector3.Distance(_trackedController2.transform.position, calibrationCube.position) < 0.25f)
        {
            initialControllerPosition = _trackedController2.transform.position;
            initialControllerRotation = _trackedController2.transform.eulerAngles;
            pointCloudManager.update = false;
        }
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        if(!pointCloudManager.update)
        {
            Vector3 positionOffset = initialControllerPosition - _trackedController2.transform.position;

            //--Add the difference of rotation to the current Zed Greenscreen rotation
            Vector3 rotationOffset = initialControllerRotation - _trackedController2.transform.eulerAngles;
            Vector3 appliedRotationoffset = rotationOffset + Zed_Greenscreen.transform.eulerAngles;

            Zed_Greenscreen.transform.position += positionOffset;
            //--rotate the Zed greenscreen towards the applied offset
            //Zed_Greenscreen.transform.eulerAngles = Vector3.RotateTowards(Zed_Greenscreen.transform.eulerAngles, appliedRotationoffset, 0.05f, 0);
            pointCloudManager.update = true;
        }
    }
}
