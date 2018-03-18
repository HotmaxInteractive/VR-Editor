using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class presentationToolsManager : MonoBehaviour
{
    private List<GameObject> presentationTools = new List<GameObject>();
    private GameObject keyingTool;
    private GameObject actorMonitor;
    private GameObject offsetCalibrator;

    void Start()
    {
        actorMonitor = transform.Find("actorMonitor").gameObject;
        keyingTool = transform.Find("keyingTool").gameObject;
        offsetCalibrator = transform.Find("offsetCalibrator").gameObject;

        presentationTools.Add(keyingTool);
        presentationTools.Add(actorMonitor);
        presentationTools.Add(offsetCalibrator);
    }

    public void activateTool(string activeTool)
    {
        foreach(GameObject keyingTool in presentationTools)
        {
            keyingTool.SetActive(false);
        }

        switch (activeTool)
        {
            case "actorMonitor":
                actorMonitor.SetActive(true);
                break;
            case "keyingTool":
                actorMonitor.SetActive(true);
                keyingTool.SetActive(true);
                break;
            case "offsetCalibrator":
                offsetCalibrator.SetActive(true);
                break;
        }
    }
}
