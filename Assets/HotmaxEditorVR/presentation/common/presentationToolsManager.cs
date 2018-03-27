using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class presentationToolsManager : MonoBehaviour
{
    //--local refs
    private bool _arModeIsOn = stateManager.arModeIsOn;

    public List<GameObject> presentationTools = new List<GameObject>();

    private GreenScreenManager greenScreenManager;

    private void Start()
    {
        greenScreenManager = FindObjectOfType<GreenScreenManager>();
        stateManager.arModeIsOnEvent += updateARModeIsOn;

        if (_arModeIsOn)
        {
            //set range and smoothness green screen vals to 0
            greenScreenManager.range = 0;
            greenScreenManager.smoothness = 0;
            greenScreenManager.UpdateShader();
        }

        //--tools must start on to initialize
        deactivateAllTools();
    }

    private void OnApplicationQuit()
    {
        stateManager.arModeIsOnEvent -= updateARModeIsOn;
    }

    void updateARModeIsOn(bool value)
    {
        _arModeIsOn = value;
        if (_arModeIsOn)
        {
            //set range and smoothness green screen vals to 0
            greenScreenManager.range = 0;
            greenScreenManager.smoothness = 0;
            greenScreenManager.UpdateShader();
        }
    }

    public void activateTool(GameObject activeTool)
    {
        deactivateAllTools();

        if (activeTool.name == presentationTools[0].name)
        {
            if (!_arModeIsOn)
            {
                presentationTools[0].SetActive(true);
            }
            else
            {
                print("Keying tools are disabled in AR Mode");
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

    void deactivateAllTools()
    {
        foreach (GameObject presentationTool in presentationTools)
        {
            presentationTool.SetActive(false);
        }
    }
}
