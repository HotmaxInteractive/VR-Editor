using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fastCalibrationSetup : MonoBehaviour
{
    public Transform Zed_Greenscreen;

    private SteamVR_TrackedController _trackedController2;

    private Vector3 initialControllerPosition;
    private Quaternion initialControllerRotation;

    public ZEDPointCloudManager pointCloudManager;

    public Transform posCalibrationCube;
    public Transform rotCalibrationCube;

    public Transform virtualCameraPos;

    private bool nearRotCube = false;
    private bool nearPosCube = false;

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
        //on unclick apply the difference in before and after to Zed Greenscreen
        if (Vector3.Distance(_trackedController2.transform.position, posCalibrationCube.position) < 0.1f && posCalibrationCube.gameObject.activeInHierarchy)
        {
            nearPosCube = true;
            initialControllerPosition = _trackedController2.transform.position;
            pointCloudManager.update = false;
        }

        if (Vector3.Distance(_trackedController2.transform.position, rotCalibrationCube.position) < 0.1f && rotCalibrationCube.gameObject.activeInHierarchy)
        {
            nearRotCube = true;
            initialControllerRotation = _trackedController2.transform.rotation;
            pointCloudManager.update = false;
        }
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        if (!pointCloudManager.update)
        {
            if(nearPosCube)
            {
                //--find the difference between CONTROLLER initial pos and current pos
                Vector3 positionOffset = initialControllerPosition - _trackedController2.transform.position;
                Zed_Greenscreen.transform.position += positionOffset * 0.2f;
                virtualCameraPos.position = Zed_Greenscreen.transform.position;
            }
            else if(nearRotCube)
            {
                //--find difference in rotation
                Quaternion rotationOffset = initialControllerRotation * Quaternion.Inverse(_trackedController2.transform.rotation); //deltaC = A * Quaternion.Inverse(B);
                Quaternion appliedDifference = rotationOffset * Zed_Greenscreen.transform.rotation; //add difference to D = C * D;

                // The step size is equal to speed times frame time.
                float step = 1 * Time.deltaTime;

                Vector3 newDir = Vector3.RotateTowards(Zed_Greenscreen.transform.forward, virtualCameraPos.forward, step, 0.0f);

                // Move our position a step closer to the target.
                Zed_Greenscreen.transform.rotation = Quaternion.LookRotation(newDir);
                virtualCameraPos.eulerAngles = appliedDifference.eulerAngles;
            }
        }

        pointCloudManager.update = true;
        nearPosCube = false;
        nearRotCube = false;
    }
}
