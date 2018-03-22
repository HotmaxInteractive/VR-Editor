using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class presentationToolsManager : MonoBehaviour
{
    public List<GameObject> presentationTools = new List<GameObject>();

    public void activateTool(GameObject activeTool)
    {
        foreach (GameObject presentationTool in presentationTools)
        {
            presentationTool.SetActive(false);
        }

        switch (activeTool.name)
        {
            case "keyingTool":
                presentationTools[0].SetActive(true);
                break;
            case "offsetCalibrator":
                presentationTools[1].SetActive(true);
                break;
            case "preprocessing":
                presentationTools[2].SetActive(true);
                break;
        }
    }
}
