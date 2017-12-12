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

    //--------------------------------------------------------------------------------------------------------------------
    private GameObject oldHitSlice;

    void Update()
    {
        //Additive if your raycast doesn't leave the pie (+ or - 15 degrees).
        RaycastHit hit;
        Ray ray = new Ray(inputManager.hand2.gameObject.transform.position, inputManager.hand2.gameObject.transform.forward);
        //Only hit the layer that has the pie slices
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Gizmo Layer")))
        {
            
            if (hit.collider != null && hit.collider.transform.parent.name == "pieHolder")
            {
                if (hit.collider.gameObject != oldHitSlice)
                {
                    float rotateDegrees = 360 / (float)hit.transform.parent.childCount;
                    Vector3 rotationYDirection = new Vector3(0, rotateDegrees, 0);
                    Vector3 rotationZDirection = new Vector3(0, 0, rotateDegrees);
                    Transform initialParent = transform.parent;

                    if (_rotationGizmoIsSelected)
                    {
                        //Going to destroy the cube after the if statement
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = transform.position;
                        int newHitSiblingIndex = hit.collider.transform.GetSiblingIndex();
                        int oldHitSiblingIndex = oldHitSlice.transform.GetSiblingIndex();
                        int numOfSlices = hit.collider.transform.parent.transform.childCount;

                        switch (_rotationMode)
                        {
                            case stateManager.rotationModes.xRotationMode:
                                //compare the current hit sibling index with old hit sibling index
                                if (newHitSiblingIndex > oldHitSiblingIndex || (newHitSiblingIndex == 0 && oldHitSiblingIndex == numOfSlices - 1))
                                {
                                    cube.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
                                    transform.parent = cube.transform;
                                    transform.localEulerAngles += rotationYDirection;
                                }
                                else {
                                    cube.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
                                    transform.parent = cube.transform;
                                    transform.localEulerAngles -= rotationYDirection;
                                }
                                break;
                            case stateManager.rotationModes.yRotationMode:
                                if (newHitSiblingIndex > oldHitSiblingIndex || (newHitSiblingIndex == 0 && oldHitSiblingIndex == numOfSlices - 1))
                                {
                                    cube.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                                    transform.parent = cube.transform;
                                    transform.localEulerAngles += rotationYDirection;
                                }
                                else
                                {
                                    cube.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                                    transform.parent = cube.transform;
                                    transform.localEulerAngles -= rotationYDirection;
                                }
                                break;
                            case stateManager.rotationModes.zRotationMode:
                                if (newHitSiblingIndex > oldHitSiblingIndex || (newHitSiblingIndex == 0 && oldHitSiblingIndex == numOfSlices - 1))
                                {
                                    cube.transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z + 90);
                                    transform.parent = cube.transform;
                                    transform.localEulerAngles += rotationZDirection;
                                }
                                else
                                {
                                    cube.transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z + 90);
                                    transform.parent = cube.transform;
                                    transform.localEulerAngles -= rotationZDirection;
                                }
                                break;
                        }
                        oldHitSlice = hit.collider.gameObject;

                        transform.parent = initialParent;

                        Destroy(cube);
                    }
                }
            }
        }
    }


    //--------------------------------------------------------------------------------------------------------------------


    private void OnEnable()
    {
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
            RaycastHit hit;
            Ray ray = new Ray(inputManager.hand2.gameObject.transform.position, inputManager.hand2.gameObject.transform.forward);
            if (Physics.Raycast(ray, out hit))
            {
                //rotate the pie slice's parent 
                oldHitSlice = hit.collider.gameObject;
            }

            switch (_rotationMode)
            {
                case stateManager.rotationModes.xRotationMode:
                    setRotationGizmoVisible(visibleRotation.X);
                    init.rotationGizmos.transform.Find("xRotationGizmo").transform.Find("pieHolder").gameObject.SetActive(true);
                    break;
                case stateManager.rotationModes.yRotationMode:
                    setRotationGizmoVisible(visibleRotation.Y);
                    init.rotationGizmos.transform.Find("yRotationGizmo").transform.Find("pieHolder").gameObject.SetActive(true);
                    break;
                case stateManager.rotationModes.zRotationMode:
                    setRotationGizmoVisible(visibleRotation.Z);
                    init.rotationGizmos.transform.Find("zRotationGizmo").transform.Find("pieHolder").gameObject.SetActive(true);

                    break;
            }
        }
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        init._stateManagerMutatorRef.SET_ROTATION_GIZMO_IS_SELECTED(false);
        init.rotationGizmos.transform.Find("xRotationGizmo").transform.Find("pieHolder").gameObject.SetActive(false);
        init.rotationGizmos.transform.Find("yRotationGizmo").transform.Find("pieHolder").gameObject.SetActive(false);
        init.rotationGizmos.transform.Find("zRotationGizmo").transform.Find("pieHolder").gameObject.SetActive(false);
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
