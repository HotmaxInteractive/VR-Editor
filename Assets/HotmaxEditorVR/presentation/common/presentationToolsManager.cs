using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class presentationToolsManager : MonoBehaviour
{
    //--local refs
    private bool arModeIsOn = true;

    public List<GameObject> presentationTools = new List<GameObject>();

    private GreenScreenManager greenScreenManager;

    public newKeyingTool keyingTool;

    private void Start()
    {
        greenScreenManager = FindObjectOfType<GreenScreenManager>();
        handleCameraSettingModes();
        deactivateAllTools();
    }

    public void activateTool(GameObject activeTool)
    {
        deactivateAllTools();

        if (activeTool.name == presentationTools[0].name)
        {
            if (!arModeIsOn)
            {
                presentationTools[0].SetActive(true);
            }
        }
        else if (activeTool.name == presentationTools[1].name)
        {
            presentationTools[1].SetActive(true);
        }
        else if (activeTool.name == presentationTools[2].name)
        {
            presentationTools[2].SetActive(true);
        }
    }

    public void deactivateAllTools()
    {
        foreach (GameObject presentationTool in presentationTools)
        {
            presentationTool.SetActive(false);
        }
    }

    public void toggleARandGreenScreen()
    {
        arModeIsOn = !arModeIsOn;
        handleCameraSettingModes();
        deactivateAllTools();
    }

    void setARCameraSettings()
    {
        //set range / smoothing to 0
        greenScreenManager.range = 0;
        greenScreenManager.smoothness = 0;

        //--set settings to automatic whitebal and exposure
        //--set preprocessing all to default vals 5,0,0,4,-1,-1,-1
        sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.BRIGHTNESS, 5);
        sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.SATURATION, 4);
        sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.CONTRAST, 0);
        sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.HUE, 0);
        sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.WHITEBALANCE, -1, true);
        sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.EXPOSURE, -1, true);
        sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.GAIN, -1, true);

        greenScreenManager.UpdateShader();
    }

    void handleCameraSettingModes()
    {
        //set default
        if (arModeIsOn)
        {
            setARCameraSettings();
        }
        else
        {
            //load JSON
            loadChromaKeyData();
        }
    }

    void loadChromaKeyData()
    {
        keyingTool.gameObject.SetActive(true);
        keyingTool.loadChromaKeyData();
        keyingTool.gameObject.SetActive(false);
    }
}
