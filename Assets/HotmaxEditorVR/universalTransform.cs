using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class universalTransform : MonoBehaviour
{
    private float initialPadYPosition;
    private float currentPadYPos;

    private float distToController;
    [SerializeField]
    private float scrollSpeed = .2f;
    private float scrollDistance = 0;

    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private bool _rotationGizmoIsSelected = stateManager.rotationGizmoIsSelected;

    private stateManager.rotationModes _rotationMode = stateManager.rotationMode;
    private stateManager.editorModes _editorMode = stateManager.editorMode;

    private float initialXControllerPosition;
    private float currentXControllerPosition;

    private float initialYControllerPosition;
    private float currentYControllerPosition;

    private List<Transform> transforms = new List<Transform>();

    private enum visibleRotation
    {
        X,
        Y,
        Z,
        ALL,
        NONE
    }


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
        //the purpose being to start the object off at the same place it was in before selecting it
        initialPadYPosition = inputManager.selectorHand.GetAxis().y;
        distToController = Vector3.Distance(transform.position, inputManager.hand2.transform.position);
        scrollDistance = 0;
    }


    void updateRotationGizmoIsSelected(bool value)
    {
        _rotationGizmoIsSelected = value;
        initialXControllerPosition = inputManager.hand2.transform.position.x;
        initialYControllerPosition = inputManager.hand2.transform.position.y;
        setRotationGizmoVisible(visibleRotation.ALL);
    }

    void updateRotationModeEvent(stateManager.rotationModes value)
    {
        _rotationMode = value;
        if (_rotationGizmoIsSelected)
        {
            switch (_rotationMode)
            {
                case stateManager.rotationModes.xRotationMode:
                    setRotationGizmoVisible(visibleRotation.X);
                    break;
                case stateManager.rotationModes.yRotationMode:
                    setRotationGizmoVisible(visibleRotation.Y);
                    break;
                case stateManager.rotationModes.zRotationMode:
                    setRotationGizmoVisible(visibleRotation.Z);
                    break;
            }
        }
    }

    void updateEditorMode(stateManager.editorModes value)
    {
        _editorMode = value;
    }

    void setRotationGizmoVisible(visibleRotation visibleRotation)
    {
        Transform xGizmo = init.rotationGizmos.transform.Find("xRotationGizmo");
        Transform yGizmo = init.rotationGizmos.transform.Find("yRotationGizmo");
        Transform zGizmo = init.rotationGizmos.transform.Find("zRotationGizmo");

        transforms.Add(xGizmo);
        transforms.Add(yGizmo);
        transforms.Add(zGizmo);


        switch (visibleRotation)
        {
            case visibleRotation.X:
                for (int i = 0; i < transforms.Count; i++)
                {
                    transforms[i].gameObject.SetActive(false);
                }
                transforms[0].gameObject.SetActive(true);
                break;

            case visibleRotation.Y:
                for (int i = 0; i < transforms.Count; i++)
                {
                    transforms[i].gameObject.SetActive(false);
                }
                transforms[1].gameObject.SetActive(true);
                break;

            case visibleRotation.Z:
                for (int i = 0; i < transforms.Count; i++)
                {
                    transforms[i].gameObject.SetActive(false);
                }
                transforms[2].gameObject.SetActive(true);
                break;

            case visibleRotation.NONE:
                for (int i = 0; i < transforms.Count; i++)
                {
                    transforms[i].gameObject.SetActive(false);
                }
                break;

            case visibleRotation.ALL:
                for (int i = 0; i < transforms.Count; i++)
                {
                    transforms[i].gameObject.SetActive(true);
                }
                break;

        }
    }


    void Update()
    {
        // -- Keep class on, but check Store to turn on functionality
        // -- else block turns off gizmo
        if (_editorMode == stateManager.editorModes.universalTransformMode)
        {
            telekinesisMode();
            rotationMode();
        }
        else
        {
            init.rotationGizmos.SetActive(false);
        }
    }


    void telekinesisMode()
    {
        //Telekinesis Mode 
        if (_selectedObjectIsActive)
        {
            init.rotationGizmos.SetActive(false);

            currentPadYPos = inputManager.selectorHand.GetAxis().y;

            Vector3 offset = inputManager.hand2.transform.forward * (distToController + scrollDistance);
            transform.position = inputManager.hand2.transform.position + offset;

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
            init.rotationGizmos.SetActive(true);
            init.rotationGizmos.transform.position = transform.position;
            init.rotationGizmos.transform.rotation = transform.rotation;
        }
    }

    void rotationMode()
    {
        //Rotation Mode
        if (_rotationGizmoIsSelected)
        {
            currentXControllerPosition = inputManager.hand2.transform.localPosition.x;
            currentYControllerPosition = inputManager.hand2.transform.localPosition.y;
            float moveBuffer = 0.01f;

            if (_rotationMode == stateManager.rotationModes.xRotationMode)
            {

                Vector3 xRotation = new Vector3(15, 0, 0);

                if (currentXControllerPosition > initialXControllerPosition + moveBuffer)
                {
                    transform.localEulerAngles -= xRotation;
                    init.rotationGizmos.transform.localEulerAngles -= xRotation;
                    initialXControllerPosition = currentXControllerPosition;
                }
                if (currentXControllerPosition < initialXControllerPosition - moveBuffer)
                {
                    transform.localEulerAngles += xRotation;
                    init.rotationGizmos.transform.localEulerAngles += xRotation;
                    initialXControllerPosition = currentXControllerPosition;
                }
            }
            if (_rotationMode == stateManager.rotationModes.yRotationMode)
            {
                Vector3 yRotation = new Vector3(0, 15, 0);
                if (currentXControllerPosition > initialXControllerPosition + moveBuffer)
                {
                    transform.localEulerAngles -= yRotation;
                    init.rotationGizmos.transform.localEulerAngles -= yRotation;
                    initialXControllerPosition = currentXControllerPosition;
                }
                if (currentXControllerPosition < initialXControllerPosition - moveBuffer)
                {
                    transform.localEulerAngles += yRotation;
                    init.rotationGizmos.transform.localEulerAngles += yRotation;
                    initialXControllerPosition = currentXControllerPosition;
                }
            }
            if (_rotationMode == stateManager.rotationModes.zRotationMode)
            {
                Vector3 zRotation = new Vector3(0, 0, 15);
                if (currentYControllerPosition > initialYControllerPosition + moveBuffer)
                {
                    transform.localEulerAngles -= zRotation;
                    init.rotationGizmos.transform.localEulerAngles -= zRotation;
                    initialYControllerPosition = currentYControllerPosition;
                }
                if (currentYControllerPosition < initialYControllerPosition - moveBuffer)
                {
                    transform.localEulerAngles += zRotation;
                    init.rotationGizmos.transform.localEulerAngles += zRotation;
                    initialYControllerPosition = currentYControllerPosition;
                }
            }
        }
    }
}