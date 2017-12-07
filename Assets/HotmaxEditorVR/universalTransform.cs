using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class universalTransform : MonoBehaviour
{
    public objectSelect objSelect;

    float initialPadYPosition;
    float currentPadYPos;

    float distToController;
    float scrollSpeed = .2f;
    public float scrollDistance = 0;

    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private bool _rotationGizmoIsSelected = stateManager.rotationGizmoIsSelected;

    private stateManager.rotationModes _rotationMode = stateManager.rotationMode;
    private stateManager.editorModes _editorMode = stateManager.editorMode;


    float initialXControllerPosition;
    float currentXControllerPosition;

    float initialYControllerPosition;
    float currentYControllerPosition;

    private List<Transform> transforms = new List<Transform>();

    private void OnEnable()
    {
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
        stateManager.rotationGizmoIsSelectedEvent += updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent += updateRotationModeEvent;
        stateManager.editorModeEvent += updateEditorMode;
    }

    protected virtual void OnDisable()
    {
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
        stateManager.rotationGizmoIsSelectedEvent -= updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent -= updateRotationModeEvent;
        stateManager.editorModeEvent -= updateEditorMode;
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;

        //this gets the initial offset of the object to the controller
        initialPadYPosition = objSelect.inputManagerRef.scrollY;
        distToController = Vector3.Distance(transform.position, objSelect.hand2.transform.position);
        scrollDistance = 0;
    }


    void updateRotationGizmoIsSelected(bool value)
    {
        _rotationGizmoIsSelected = value;
        initialXControllerPosition = objSelect.hand2.transform.position.x;
        initialYControllerPosition = objSelect.hand2.transform.position.y;
        setAllRotationGizmosVisible(true);
    }

    void updateRotationModeEvent(stateManager.rotationModes value)
    {
        _rotationMode = value;
        if (_rotationGizmoIsSelected)
        {
            if (_rotationMode == stateManager.rotationModes.xRotationMode)
            {
                setOneRotationGizmoVisible(0);
            }
            if (_rotationMode == stateManager.rotationModes.yRotationMode)
            {
                setOneRotationGizmoVisible(1);
            }
            if (_rotationMode == stateManager.rotationModes.zRotationMode)
            {
                setOneRotationGizmoVisible(2);
            }
        }
    }

    void updateEditorMode(stateManager.editorModes value)
    {
        _editorMode = value;
    }

    void setOneRotationGizmoVisible(int gizmo)
    {
        Transform xGizmo = objSelect.rotationGizmos.transform.Find("xRotationGizmo");
        Transform yGizmo = objSelect.rotationGizmos.transform.Find("yRotationGizmo");
        Transform zGizmo = objSelect.rotationGizmos.transform.Find("zRotationGizmo");

        transforms.Add(xGizmo);
        transforms.Add(yGizmo);
        transforms.Add(zGizmo);

        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].gameObject.SetActive(false);
        }
        transforms[gizmo].gameObject.SetActive(true);

    }

    void setAllRotationGizmosVisible(bool setVisible)
    {
        Transform xGizmo = objSelect.rotationGizmos.transform.Find("xRotationGizmo");
        Transform yGizmo = objSelect.rotationGizmos.transform.Find("yRotationGizmo");
        Transform zGizmo = objSelect.rotationGizmos.transform.Find("zRotationGizmo");

        transforms.Add(xGizmo);
        transforms.Add(yGizmo);
        transforms.Add(zGizmo);

        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].gameObject.SetActive(setVisible);
        }
    }

    void Update()
    {
        // -- Keep class on, but check Store to turn on functionality
        // -- else block turns off gizmo
        if (_editorMode == stateManager.editorModes.universalTransformMode)
        {
            //Telekinesis Mode 
            if (_selectedObjectIsActive)
            {
                objSelect.rotationGizmos.SetActive(false);

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
            else
            {
                objSelect.rotationGizmos.SetActive(true);
                objSelect.rotationGizmos.transform.position = transform.position;
                objSelect.rotationGizmos.transform.rotation = transform.rotation;
            }

            //Rotation Mode
            if (_rotationGizmoIsSelected)
            {
                currentXControllerPosition = objSelect.hand2.transform.localPosition.x;
                currentYControllerPosition = objSelect.hand2.transform.localPosition.y;
                float moveBuffer = 0.01f;

                if (_rotationMode == stateManager.rotationModes.xRotationMode)
                {
  
                    Vector3 xRotation = new Vector3(15, 0, 0);

                    if (currentXControllerPosition > initialXControllerPosition + moveBuffer)
                    {
                        transform.localEulerAngles -= xRotation;
                        objSelect.rotationGizmos.transform.localEulerAngles -= xRotation;
                        initialXControllerPosition = currentXControllerPosition;
                    }
                    if (currentXControllerPosition < initialXControllerPosition - moveBuffer)
                    {
                        transform.localEulerAngles += xRotation;
                        objSelect.rotationGizmos.transform.localEulerAngles += xRotation;
                        initialXControllerPosition = currentXControllerPosition;
                    }
                }
                if (_rotationMode == stateManager.rotationModes.yRotationMode)
                {
                    Vector3 yRotation = new Vector3(0, 15, 0);
                    if (currentXControllerPosition > initialXControllerPosition + moveBuffer)
                    {
                        transform.localEulerAngles -= yRotation;
                        objSelect.rotationGizmos.transform.localEulerAngles -= yRotation;
                        initialXControllerPosition = currentXControllerPosition;
                    }
                    if (currentXControllerPosition < initialXControllerPosition - moveBuffer)
                    {
                        transform.localEulerAngles += yRotation;
                        objSelect.rotationGizmos.transform.localEulerAngles += yRotation;
                        initialXControllerPosition = currentXControllerPosition;
                    }
                }
                if (_rotationMode == stateManager.rotationModes.zRotationMode)
                {
                    Vector3 zRotation = new Vector3(0, 0, 15);
                    if (currentYControllerPosition > initialYControllerPosition + moveBuffer)
                    {
                        transform.localEulerAngles -= zRotation;
                        objSelect.rotationGizmos.transform.localEulerAngles -= zRotation;
                        initialYControllerPosition = currentYControllerPosition;
                    }
                    if (currentYControllerPosition < initialYControllerPosition - moveBuffer)
                    {
                        transform.localEulerAngles += zRotation;
                        objSelect.rotationGizmos.transform.localEulerAngles += zRotation;
                        initialYControllerPosition = currentYControllerPosition;
                    }
                }
            }
        }
        else
        {
            objSelect.rotationGizmos.SetActive(false);
        }
    }
}