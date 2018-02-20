using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyingToolToggle : MonoBehaviour
{

    private keyingTool keyingTool;

    private void Start()
    {
        keyingTool = FindObjectOfType<keyingTool>();
    }

    public void toggleChromaKeyTools()
    {
        if (keyingTool.gameObject.activeInHierarchy)
        {
            keyingTool.gameObject.SetActive(false);
        }
        else
        {
            keyingTool.gameObject.SetActive(true);
        }
    }
}
