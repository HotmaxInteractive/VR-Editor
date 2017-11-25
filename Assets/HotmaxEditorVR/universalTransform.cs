using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class universalTransform : MonoBehaviour
{
    public objectSelect objSelect;
    editStateController stateController;

    float initialPadYPosition;
    float currentPadYPos;

    float distToController;
    float scrollSpeed = .2f;
    public float scrollDistance = 0;

    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private bool _rotationGizmoIsActive = stateManager.rotationGizmosActive;
    private bool _rotationGizmoIsSelected = stateManager.rotationGizmoIsSelected;
    private stateManager.rotationModes _rotationMode = stateManager.rotationMode;

    float initialControllerPosition;
    float currentControllerPosition;

    private void OnEnable()
    {
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
        stateManager.rotationGizmosActiveEvent += updateRotationGizmoEvent;
        stateManager.rotationGizmoIsSelectedEvent += updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent += updateRotationModeEvent;

        initialPadYPosition = objSelect.inputManagerRef.scrollY;
        distToController = Vector3.Distance(transform.position, objSelect.hand2.transform.position);
        scrollDistance = 0;
    }

    protected virtual void OnDisable()
    {
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
        stateManager.rotationGizmosActiveEvent -= updateRotationGizmoEvent;
        stateManager.rotationGizmoIsSelectedEvent -= updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent -= updateRotationModeEvent;
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
    }

    void updateRotationGizmoEvent(bool value)
    {
        _rotationGizmoIsActive = value;
        if(_rotationGizmoIsActive)
        {
            objSelect.rotationGizmos.SetActive(value);
            objSelect.rotationGizmos.transform.position = transform.position;
            objSelect.rotationGizmos.transform.rotation = transform.rotation;       
        }
        else
        {
            objSelect.rotationGizmos.SetActive(value);
        }
    }

    void updateRotationGizmoIsSelected(bool value)
    {
        _rotationGizmoIsSelected = value;
        initialControllerPosition = objSelect.hand2.transform.position.x;
    }

    void updateRotationModeEvent(stateManager.rotationModes value)
    {
        _rotationMode = value;
    }

    void Update()
    {
        if (_selectedObjectIsActive)
        {
            currentPadYPos = objSelect.inputManagerRef.scrollY;

            Vector3 offset = objSelect.hand2.transform.forward * (distToController + scrollDistance);
            transform.position = objSelect.hand2.transform.position + offset;

            if (currentPadYPos > initialPadYPosition)
            {
                scrollDistance += scrollSpeed;
                initialPadYPosition = currentPadYPos;
            }
            if (currentPadYPos < initialPadYPosition)
            {
                scrollDistance -= scrollSpeed;
                initialPadYPosition = currentPadYPos;
            }
        }

        if (_rotationGizmoIsSelected)
        {
            currentControllerPosition = objSelect.hand2.transform.localPosition.z;

            if(_rotationMode == stateManager.rotationModes.xRotationMode)
            {
                if (currentControllerPosition > initialControllerPosition + .01f)
                {
                    transform.localEulerAngles -= new Vector3(15, 0, 0);
                    objSelect.rotationGizmos.transform.localEulerAngles -= new Vector3(15, 0, 0);
                    initialControllerPosition = currentControllerPosition;
                }
                if (currentControllerPosition < initialControllerPosition - .01f)
                {
                    transform.localEulerAngles += new Vector3(15, 0, 0);
                    objSelect.rotationGizmos.transform.localEulerAngles += new Vector3(15, 0, 0);
                    initialControllerPosition = currentControllerPosition;
                }
            }
            if (_rotationMode == stateManager.rotationModes.yRotationMode)
            {
                if (currentControllerPosition > initialControllerPosition + .01f)
                {
                    transform.localEulerAngles -= new Vector3(0, 15, 0);
                    objSelect.rotationGizmos.transform.localEulerAngles -= new Vector3(0, 15, 0);
                    initialControllerPosition = currentControllerPosition;
                }
                if (currentControllerPosition < initialControllerPosition - .01f)
                {
                    transform.localEulerAngles += new Vector3(0, 15, 0);
                    objSelect.rotationGizmos.transform.localEulerAngles += new Vector3(0, 15, 0);
                    initialControllerPosition = currentControllerPosition;
                }
            }
            if (_rotationMode == stateManager.rotationModes.zRotationMode)
            {
                if (currentControllerPosition > initialControllerPosition + .01f)
                {
                    transform.localEulerAngles -= new Vector3(0, 0, 15);
                    objSelect.rotationGizmos.transform.localEulerAngles -= new Vector3(0, 0, 15);
                    initialControllerPosition = currentControllerPosition;
                }
                if (currentControllerPosition < initialControllerPosition - .01f)
                {
                    transform.localEulerAngles += new Vector3(0, 0, 15);
                    objSelect.rotationGizmos.transform.localEulerAngles += new Vector3(0, 0, 15);
                    initialControllerPosition = currentControllerPosition;
                }
            }
        }
    }
}