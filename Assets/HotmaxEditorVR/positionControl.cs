using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionControl : MonoBehaviour
{
    public objectSelect objSelect;
    editStateController stateController;

    private void Start()
    {
        stateController = GetComponent<editStateController>();
    }

    void Update()
    {
        if (objSelect.trackedController2.triggerPressed)
        {
            transform.position = objSelect.endPosition;
        }
    }
}
