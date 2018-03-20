using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class accessCamSettingsManager : MonoBehaviour
{

    ZEDCameraSettingsManager cameraSettingsManager = new ZEDCameraSettingsManager();
    ZEDCameraSettingsManager.CameraSettings cameraSettings = new ZEDCameraSettingsManager.CameraSettings();

    private string path = "ZED_Settings.conf";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cameraSettings.Brightness = 8;
            SaveCameraSettings(path);
            cameraSettingsManager.LoadCameraSettings(sl.ZEDCamera.GetInstance(), path);
            cameraSettingsManager.SetSettings(sl.ZEDCamera.GetInstance());
            print(System.IO.File.ReadAllLines(path));
        }
    }

    public void SaveCameraSettings(string path)
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        {
            file.WriteLine("brightness=" + cameraSettings.Brightness.ToString());
            file.WriteLine("contrast=" + cameraSettings.Contrast.ToString());
            file.WriteLine("hue=" + cameraSettings.Hue.ToString());
            file.WriteLine("saturation=" + cameraSettings.Saturation.ToString());
            file.WriteLine("whiteBalance=" + cameraSettings.WhiteBalance.ToString());
            file.WriteLine("gain=" + cameraSettings.Gain.ToString());
            file.WriteLine("exposure=" + cameraSettings.Exposure.ToString());
            file.Close();
        }
    }
}
