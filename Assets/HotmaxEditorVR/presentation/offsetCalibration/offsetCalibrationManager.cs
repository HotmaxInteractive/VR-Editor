using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offsetCalibrationManager : MonoBehaviour
{
    //--TODO: probably should put an object that follows the camera when the position is reset
    //--TODO: needs to have public events for toggling the setup mode (fast and manual)

    public GameObject fastCalibrationSetup;
    public GameObject manualCalibrationSetup;

    public void enableFastCalibrationSetup()
    {
        fastCalibrationSetup.SetActive(true);
        manualCalibrationSetup.SetActive(false);
    }

    public void enableManualCalibrationSetup()
    {
        manualCalibrationSetup.SetActive(true);
        fastCalibrationSetup.SetActive(false);
    }
}
