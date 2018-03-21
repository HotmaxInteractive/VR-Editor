using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manualCalibrationSetup : MonoBehaviour
{
    public Transform Zed_Greenscreen;
    private bool isRotation = true;
    public GameObject rotationText;
    public GameObject positionText;

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
