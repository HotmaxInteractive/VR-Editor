using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationControl : MonoBehaviour
{
    private bool _rotationGizmoIsSelected = stateManager.rotationGizmoIsSelected;

    private stateManager.rotationModes _rotationMode = stateManager.rotationMode;

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

    private void Awake()
    {
        print("rot control is awake: " + gameObject.name);
    }

    private void OnEnable()
    {
        print("rotational control is enabled: " + gameObject.name);
        stateManager.rotationGizmoIsSelectedEvent += updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent += updateRotationModeEvent;

        //handle the visual representation of the rotation gizmo
        init.rotationGizmos.SetActive(true);
        init.rotationGizmos.transform.position = transform.position;
        init.rotationGizmos.transform.rotation = transform.rotation;

        inputManager.trackedController2.TriggerUnclicked += triggerUnclicked;
    }

    protected virtual void OnDisable()
    {
        print("rotational control is disabled: " + gameObject.name);

        stateManager.rotationGizmoIsSelectedEvent -= updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent -= updateRotationModeEvent;
        init.rotationGizmos.SetActive(false);

        inputManager.trackedController2.TriggerUnclicked -= triggerUnclicked;
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

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        init._stateManagerMutatorRef.SET_ROTATION_GIZMO_IS_SELECTED(false);
    }

    void Update()
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
}
