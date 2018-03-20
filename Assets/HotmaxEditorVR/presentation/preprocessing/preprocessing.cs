using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class preprocessing : MonoBehaviour
{
    private string preprocessingDataFile = "ZED_Settings.conf";

    private ZEDCameraSettingsManager zedCameraSettingsManager = new ZEDCameraSettingsManager();
    private ZEDCameraSettingsManager.CameraSettings cameraSettings = new ZEDCameraSettingsManager.CameraSettings();

    private SteamVR_TrackedController _trackedController2;

    private int brightnessValue;
    private int contrastValue;
    private int hueValue;
    private int saturationValue;
    //private int whiteBalanceValue;
    //private int gainValue;
    //private int exposureValue;

    public TextMeshPro brightnessText;
    public TextMeshPro contrastText;
    public TextMeshPro hueText;
    public TextMeshPro saturationText;

    public Transform brightnessSlider;
    public Transform contrastSlider;
    public Transform hueSlider;
    public Transform saturationSlider;
    //public Transform whiteBalanceSlider;
    //public Transform gainSlider;
    //public Transform exposureSlider;

    private void Start()
    {
        _trackedController2 = inputManager.trackedController2;
        loadPreprocessingFX();
    }

    void Update()
    {
        setPreprocessingValues();
        updateTextValues();
    }

    void setPreprocessingValues()
    {
        if (_trackedController2.triggerPressed)
        {
            //setting preFX values
            brightnessValue = Mathf.RoundToInt(brightnessSlider.localPosition.x * 8);
            contrastValue = Mathf.RoundToInt(contrastSlider.localPosition.x * 8);
            hueValue = Mathf.RoundToInt(hueSlider.localPosition.x * 11);
            saturationValue = Mathf.RoundToInt(saturationSlider.localPosition.x * 8);

            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.BRIGHTNESS, brightnessValue);
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.CONTRAST, contrastValue);
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.HUE, hueValue);
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.SATURATION, saturationValue);
        }
    }

    void updateSliderPositions()
    {
        brightnessSlider.localPosition = new Vector3(brightnessValue / (float)8, brightnessSlider.localPosition.y, brightnessSlider.localPosition.z);
        contrastSlider.localPosition = new Vector3(contrastValue / (float)8, contrastSlider.localPosition.y, contrastSlider.localPosition.z);
        hueSlider.localPosition = new Vector3(hueValue / (float)11, hueSlider.localPosition.y, hueSlider.localPosition.z);
        saturationSlider.localPosition = new Vector3(saturationValue / (float)8, saturationSlider.localPosition.y, saturationSlider.localPosition.z);
    }

    public void savePreprocessingFX()
    {

        using (StreamWriter file = new StreamWriter(preprocessingDataFile))
        {
            file.WriteLine("brightness=" + brightnessValue.ToString());
            file.WriteLine("contrast=" + contrastValue.ToString());
            file.WriteLine("hue=" + hueValue.ToString());
            file.WriteLine("saturation=" + saturationValue.ToString());
            //file.WriteLine("whiteBalance=" + cameraSettings.WhiteBalance.ToString());
            //file.WriteLine("gain=" + cameraSettings.Gain.ToString());
            //file.WriteLine("exposure=" + cameraSettings.Exposure.ToString());
            file.Close();
        }
    }

    public void loadPreprocessingFX()
    {
        //TODO: If no file, write defaults and then load
        string[] lines = null;
        try
        {
            lines = System.IO.File.ReadAllLines(preprocessingDataFile);
        }
        catch (System.Exception)
        {

        }
        if (lines == null) return;

        foreach (string line in lines)
        {
            string[] splittedLine = line.Split('=');
            if (splittedLine.Length == 2)
            {
                string key = splittedLine[0];
                string field = splittedLine[1];

                if (key == "brightness")
                {
                    sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.BRIGHTNESS, int.Parse(field));
                    brightnessValue = int.Parse(field);
                }
                else if (key == "contrast")
                {
                    sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.CONTRAST, int.Parse(field));
                    contrastValue = int.Parse(field);
                }
                else if (key == "hue")
                {
                    sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.HUE, int.Parse(field));
                    hueValue = int.Parse(field);
                }
                else if (key == "saturation")
                {
                    sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.SATURATION, int.Parse(field));
                    saturationValue = int.Parse(field);
                }
                else if (key == "whiteBalance")
                {
                    sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.WHITEBALANCE, int.Parse(field));
                    //brightnessValue = int.Parse(field);
                }
                else if (key == "gain")
                {
                    sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.GAIN, int.Parse(field));
                    //brightnessValue = int.Parse(field);
                }
                else if (key == "exposure")
                {
                    sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.EXPOSURE, int.Parse(field));
                    //brightnessValue = int.Parse(field);
                }
            }
        }

        updateSliderPositions();
        updateTextValues();
    }

    void updateTextValues()
    {
        brightnessText.text = "Brightness : " + brightnessValue.ToString();
        contrastText.text = "Contrast : " + contrastValue.ToString();
        hueText.text = "Hue : " + hueValue.ToString();
        saturationText.text = "Saturation : " + saturationValue.ToString();
    }
}