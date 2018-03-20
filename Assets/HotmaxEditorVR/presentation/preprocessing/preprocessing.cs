using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preprocessing : MonoBehaviour
{

    private ZEDCameraSettingsManager zedCameraSettingsManager = new ZEDCameraSettingsManager();
    private ZEDCameraSettingsManager.CameraSettings cameraSettings = new ZEDCameraSettingsManager.CameraSettings();

    private SteamVR_TrackedController _trackedController2;

    public Transform brightnessSlider;
    public Transform contrastSlider;
    public Transform hueSlider;
    public Transform saturationSlider;

    private string path = "ZED_Settings.conf";

    private void Start()
    {
        _trackedController2 = inputManager.trackedController2;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cameraSettings.Brightness = 8;
            saveCameraSettings(path);
            zedCameraSettingsManager.LoadCameraSettings(sl.ZEDCamera.GetInstance(), path);
        }

        //TODO: this is stupid, find a way to set the camera pre settings before writing to file
        if(_trackedController2.triggerPressed)
        {
            //setting preFX values
            cameraSettings.Brightness = Mathf.RoundToInt(brightnessSlider.localPosition.x * 8);
            cameraSettings.Contrast = Mathf.RoundToInt(contrastSlider.localPosition.x * 8);
            cameraSettings.Hue = Mathf.RoundToInt(hueSlider.localPosition.x * 11);
            cameraSettings.Saturation = Mathf.RoundToInt(saturationSlider.localPosition.x * 8);

            saveCameraSettings(path);
            zedCameraSettingsManager.LoadCameraSettings(sl.ZEDCamera.GetInstance(), path);
        }
    }

    public void saveCameraSettings(string path)
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