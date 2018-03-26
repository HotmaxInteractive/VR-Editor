using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class preprocessing : MonoBehaviour
{
    //--local refs
    private SteamVR_TrackedController _trackedController2;
    private bool _arModeIsOn = stateManager.arModeIsOn;

    private string preprocessingDataFile = "ZED_Settings.conf";

    private ZEDCameraSettingsManager zedCameraSettingsManager = new ZEDCameraSettingsManager();
    private ZEDCameraSettingsManager.CameraSettings cameraSettings = new ZEDCameraSettingsManager.CameraSettings();

    public GameObject arModePanel;
    public GameObject greenScreenModePanel;

    private int brightnessValue;
    private int contrastValue;
    private int hueValue;
    private int saturationValue;

    private int whiteBalanceValue;
    private int gainValue;
    private int exposureValue;

    public TextMeshPro brightnessText;
    public TextMeshPro contrastText;
    public TextMeshPro hueText;
    public TextMeshPro saturationText;

    public TextMeshPro whiteBalanceText;
    public TextMeshPro gainText;
    public TextMeshPro exposureText;

    public Transform brightnessSlider;
    public Transform contrastSlider;
    public Transform hueSlider;
    public Transform saturationSlider;

    public Transform whiteBalanceSlider;
    public Transform gainSlider;
    public Transform exposureSlider;

    private void Start()
    {
        stateManager.arModeIsOnEvent += updateARModeIsOn;

        _trackedController2 = inputManager.trackedController2;

        savePreprocessingFX();
        loadPreprocessingFX();
    }

    private void OnApplicationQuit()
    {
        stateManager.arModeIsOnEvent -= updateARModeIsOn;
    }

    void Update()
    {
        setPreprocessingValues();
        updateTextValues();
    }

    void updateARModeIsOn(bool value)
    {
        _arModeIsOn = value;
        handleAutoAdjustSettings();

        //--show the right toggle button 
        arModePanel.SetActive(false);
        greenScreenModePanel.SetActive(false);
        if (_arModeIsOn)
        {
            arModePanel.SetActive(true);
        }
        else
        {
            greenScreenModePanel.SetActive(true);
        }
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

            whiteBalanceValue = Mathf.RoundToInt(whiteBalanceSlider.localPosition.x * 65);
            gainValue = Mathf.RoundToInt(gainSlider.localPosition.x * 100);
            exposureValue = Mathf.RoundToInt(exposureSlider.localPosition.x * 100);

            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.BRIGHTNESS, brightnessValue);
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.CONTRAST, contrastValue);
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.HUE, hueValue);
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.SATURATION, saturationValue);

            if (!_arModeIsOn)
            {
                sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.WHITEBALANCE, whiteBalanceValue);
                sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.GAIN, gainValue);
                sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.EXPOSURE, exposureValue);
            }
        }
    }

    void updateSliderPositions()
    {
        brightnessSlider.localPosition = new Vector3(brightnessValue / (float)8, brightnessSlider.localPosition.y, brightnessSlider.localPosition.z);
        contrastSlider.localPosition = new Vector3(contrastValue / (float)8, contrastSlider.localPosition.y, contrastSlider.localPosition.z);
        hueSlider.localPosition = new Vector3(hueValue / (float)11, hueSlider.localPosition.y, hueSlider.localPosition.z);
        saturationSlider.localPosition = new Vector3(saturationValue / (float)8, saturationSlider.localPosition.y, saturationSlider.localPosition.z);

        if (!_arModeIsOn)
        {
            whiteBalanceSlider.localPosition = new Vector3(whiteBalanceValue / (float)65, whiteBalanceSlider.localPosition.y, whiteBalanceSlider.localPosition.z);
            gainSlider.localPosition = new Vector3(gainValue / (float)100, gainSlider.localPosition.y, gainSlider.localPosition.z);
            exposureSlider.localPosition = new Vector3(exposureValue / (float)100, exposureSlider.localPosition.y, exposureSlider.localPosition.z);
        }
    }

    public void savePreprocessingFX()
    {

        using (StreamWriter file = new StreamWriter(preprocessingDataFile))
        {
            file.WriteLine("brightness=" + brightnessValue.ToString());
            file.WriteLine("contrast=" + contrastValue.ToString());
            file.WriteLine("hue=" + hueValue.ToString());
            file.WriteLine("saturation=" + saturationValue.ToString());

            file.WriteLine("whiteBalance=" + cameraSettings.WhiteBalance.ToString());
            file.WriteLine("gain=" + cameraSettings.Gain.ToString());
            file.WriteLine("exposure=" + cameraSettings.Exposure.ToString());
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
                    whiteBalanceValue = int.Parse(field);
                }
                else if (key == "gain")
                {
                    sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.GAIN, int.Parse(field));
                    gainValue = int.Parse(field);
                }
                else if (key == "exposure")
                {
                    sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.EXPOSURE, int.Parse(field));
                    exposureValue = int.Parse(field);
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

        if (!_arModeIsOn)
        {
            whiteBalanceText.text = "White Balance : " + whiteBalanceValue.ToString();
            gainText.text = "Gain : " + gainValue.ToString();
            exposureText.text = "Exposure : " + exposureValue.ToString();
        }
        else
        {
            whiteBalanceText.text = "Auto";
            gainText.text = "Auto";
            exposureText.text = "Auto";
        }
    }

    private void handleAutoAdjustSettings()
    {
        if (_arModeIsOn)
        {
            //--turn on automatic camera settings adjust
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.WHITEBALANCE, -1, true);
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.EXPOSURE, -1, true);
        }
        else
        {
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.WHITEBALANCE, whiteBalanceValue, false);
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.EXPOSURE, exposureValue + 1, false);
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.GAIN, gainValue, false);
        }
    }
}