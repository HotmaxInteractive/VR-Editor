using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calibrationButtons : MonoBehaviour
{
    public Transform Zed_Greenscreen;
    private bool isRotation = true;
    public GameObject rotationText;
    public GameObject positionText;

    private SteamVR_TrackedController _trackedController2;
    private Vector3 initialControllerPosition;
    private Vector3 initialControllerRotation;

    private bool initialTriggerClick = false;

    public ZEDPointCloudManager pointCloudManager;

    private void Start()
    {
        _trackedController2 = inputManager.trackedController2;
        _trackedController2.TriggerClicked += triggerClicked;
    }

    private void OnApplicationQuit()
    {
        _trackedController2.TriggerClicked -= triggerClicked;
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        //save position and rotation on initial click
        //...move controller...
        //on second click apply the difference in before and after to Zed Greenscreen
        initialTriggerClick = !initialTriggerClick;

        if(initialTriggerClick)
        {
            initialControllerPosition = _trackedController2.transform.position;
            initialControllerRotation = _trackedController2.transform.eulerAngles;
            pointCloudManager.update = false;
        }
        else
        {
            Vector3 positionOffset = initialControllerPosition - _trackedController2.transform.position;

            //--Add the difference of rotation to the current Zed Greenscreen rotation
            Vector3 rotationOffset = initialControllerRotation - _trackedController2.transform.eulerAngles;
            Vector3 appliedRotationoffset = rotationOffset + Zed_Greenscreen.transform.eulerAngles;

            Zed_Greenscreen.transform.position += positionOffset;

            //--rotate the Zed greenscreen towards the applied offset
            Zed_Greenscreen.transform.eulerAngles = Vector3.RotateTowards(Zed_Greenscreen.transform.eulerAngles, appliedRotationoffset, 0.01f, 0);
            pointCloudManager.update = true;
        }
    }

    public void toggleTransformControl()
    {
        isRotation = !isRotation;

        rotationText.SetActive(false);
        positionText.SetActive(false);

        if (isRotation)
        {
            rotationText.SetActive(true);
        }
        else
        {
            positionText.SetActive(true);
        }
    }

    public void nudgeXRot(int direction)
    {
        if (isRotation)
        {
            Zed_Greenscreen.eulerAngles += new Vector3(direction, 0, 0);
        }
        else
        {
            Zed_Greenscreen.localPosition += new Vector3(direction * 0.01f, 0, 0);
        }
    }

    public void nudgeYRot(int direction)
    {
        if (isRotation)
        {
            Zed_Greenscreen.eulerAngles += new Vector3(0, direction, 0);
        }
        else
        {
            Zed_Greenscreen.localPosition += new Vector3(0, direction * 0.01f, 0);
        }
    }

    public void nudgeZRot(int direction)
    {
        if (isRotation)
        {
            Zed_Greenscreen.eulerAngles += new Vector3(0, 0, direction);
        }
        else
        {
            Zed_Greenscreen.localPosition += new Vector3(0, 0, direction * 0.01f);
        }
    }
}
