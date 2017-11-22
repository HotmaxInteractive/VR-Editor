using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionControl : MonoBehaviour
{
    public objectSelect objSelect;
    editStateController stateController;

    float initialYPosition;
    float currentYPos;

    float distToController;
    float scrollSpeed = 1f;
    public float scrollDistance = 0;

    private stateManager.inputPadModes _inputPadMode = stateManager.inputPadMode;

    private void Awake()
    {
        stateManager.inputModeEvent += updateInputMode;
    }

    protected virtual void OnApplicationQuit()
    {
        stateManager.inputModeEvent -= updateInputMode;
    }

    void updateInputMode(stateManager.inputPadModes value)
    {
        _inputPadMode = value;
    }

    private void OnEnable()
    {
        initialYPosition = objSelect.inputManagerRef.scrollY;
        distToController = Vector3.Distance(transform.position, objSelect.hand2.transform.position);
        scrollDistance = 0;
    }


    void Update()
    {
        if (_inputPadMode == stateManager.inputPadModes.tuningMode)
        {
            currentYPos = objSelect.inputManagerRef.scrollY;


            Vector3 offset = objSelect.hand2.transform.forward * (distToController + scrollDistance);
            transform.position = objSelect.hand2.transform.position + new Vector3(offset.x , offset.y, offset.z);

            if (currentYPos > initialYPosition)
            {
                scrollDistance += 1;
                initialYPosition = currentYPos;
            }
            if (currentYPos < initialYPosition)
            {
                scrollDistance -= 1;
                initialYPosition = currentYPos;
            }
        }
    }
}
