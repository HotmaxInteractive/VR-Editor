using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationControl : MonoBehaviour
{
    //--local refs
    private bool _rotationGizmoIsSelected = stateManager.rotationGizmoIsSelected;
    private stateManager.rotationModes _rotationMode = stateManager.rotationMode;

    //--vars used during rotation
    private bool initialHit = false;
    private GameObject lookAtRaycast;
    private GameObject stationaryLookAtRaycast;
    private GameObject propRotationHolder;
    private Vector3 lookPos;
    private string rotationGizmo = "RotationGizmo";
    private string rotationAxisCollider = "pie";

    //--Handles turning off and on Gizmo axis
    private List<Transform> rotationAxis = new List<Transform>();
    private enum visibleRotation
    {
        X,
        Y,
        Z,
        ALL,
        NONE
    }

    private void Start()
    {
        //holder for prop during rotation
        propRotationHolder = Instantiate(Resources.Load("propRotationHolder", typeof(GameObject))) as GameObject;
        //--rotation offset gages
        lookAtRaycast = Instantiate(Resources.Load("rotationGage", typeof(GameObject))) as GameObject;
        stationaryLookAtRaycast = Instantiate(Resources.Load("rotationGage", typeof(GameObject))) as GameObject;
    }

    private void OnEnable()
    {
        stateManager.rotationGizmoIsSelectedEvent += updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent += updateRotationModeEvent;
        inputManager.trackedController2.TriggerUnclicked += triggerUnclicked;

        init.rotationGizmos.SetActive(true);
        init.rotationGizmos.transform.position = transform.position;
    }

    protected virtual void OnDisable()
    {
        stateManager.rotationGizmoIsSelectedEvent -= updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent -= updateRotationModeEvent;
        inputManager.trackedController2.TriggerUnclicked -= triggerUnclicked;

        if (init.rotationGizmos != null)
        {
            init.rotationGizmos.SetActive(false);
        }
    }

    void updateRotationGizmoIsSelected(bool value)
    {
        _rotationGizmoIsSelected = value;
        setRotationGizmoVisible(visibleRotation.ALL);

        if (_rotationGizmoIsSelected)
        {
            lookAtRaycast.SetActive(true);
            stationaryLookAtRaycast.SetActive(true);
            lookAtRaycast.transform.position = transform.position;
            stationaryLookAtRaycast.transform.position = transform.position;
            propRotationHolder.transform.position = transform.position;

            initialHit = true;
        }
        else
        {
            //re-parent back in props
            transform.parent = init.props.transform;
            lookAtRaycast.SetActive(false);
            stationaryLookAtRaycast.SetActive(false);
        }
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
                    init.rotationGizmos.transform.Find("x" + rotationGizmo).transform.Find(rotationAxisCollider).gameObject.SetActive(true);
                    break;
                case stateManager.rotationModes.yRotationMode:
                    setRotationGizmoVisible(visibleRotation.Y);
                    init.rotationGizmos.transform.Find("y" + rotationGizmo).transform.Find(rotationAxisCollider).gameObject.SetActive(true);
                    break;
                case stateManager.rotationModes.zRotationMode:
                    setRotationGizmoVisible(visibleRotation.Z);
                    init.rotationGizmos.transform.Find("z" + rotationGizmo).transform.Find(rotationAxisCollider).gameObject.SetActive(true);
                    break;
            }
        }
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        init._stateManagerMutatorRef.SET_ROTATION_GIZMO_IS_SELECTED(false);
        init.rotationGizmos.transform.Find("x" + rotationGizmo).transform.Find(rotationAxisCollider).gameObject.SetActive(false);
        init.rotationGizmos.transform.Find("y" + rotationGizmo).transform.Find(rotationAxisCollider).gameObject.SetActive(false);
        init.rotationGizmos.transform.Find("z" + rotationGizmo).transform.Find(rotationAxisCollider).gameObject.SetActive(false);
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(inputManager.hand2.gameObject.transform.position, inputManager.hand2.gameObject.transform.forward);
        //Only hit the layer that is in Gizmo Layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Gizmo Layer")))
        {
            if (hit.collider != null)
            {
                if (_rotationGizmoIsSelected)
                {
                    switch (_rotationMode)
                    {
                        case stateManager.rotationModes.xRotationMode:
                            doRotation(hit, Vector3.right, lookPos.x);
                            break;

                        case stateManager.rotationModes.yRotationMode:
                            doRotation(hit, Vector3.up, lookPos.y);
                            break;

                        case stateManager.rotationModes.zRotationMode:
                            doRotation(hit, Vector3.forward, lookPos.z);
                            break;
                    }
                }
            }
        }
    }

    void doRotation(RaycastHit hit, Vector3 faceDirection, float lockAxis)
    {
        if (initialHit)
        {
            //--initialize holder and gage rotations / positions
            lookAtRaycast.transform.LookAt(hit.point, faceDirection);
            stationaryLookAtRaycast.transform.LookAt(hit.point, faceDirection);
            propRotationHolder.transform.LookAt(hit.point, faceDirection);

            transform.parent = propRotationHolder.transform;
            initialHit = false;
        }

        lookPos = hit.point - lookAtRaycast.transform.position;
        //locks the rotation to just one axis
        lockAxis = 0;

        Quaternion rotateDirection = Quaternion.LookRotation(lookPos, faceDirection);

        lookAtRaycast.transform.rotation = Quaternion.Slerp(lookAtRaycast.transform.rotation, rotateDirection, Time.deltaTime * 3);
        propRotationHolder.transform.rotation = Quaternion.Slerp(propRotationHolder.transform.rotation, rotateDirection, Time.deltaTime * 3);
    }

    void setRotationGizmoVisible(visibleRotation visibleRotation)
    {
        Transform xGizmo = init.rotationGizmos.transform.Find("x" + rotationGizmo);
        Transform yGizmo = init.rotationGizmos.transform.Find("y" + rotationGizmo);
        Transform zGizmo = init.rotationGizmos.transform.Find("z" + rotationGizmo);

        rotationAxis.Add(xGizmo);
        rotationAxis.Add(yGizmo);
        rotationAxis.Add(zGizmo);

        for (int i = 0; i < rotationAxis.Count; i++)
        {
            rotationAxis[i].gameObject.SetActive(false);
        }

        switch (visibleRotation)
        {
            case visibleRotation.X:
                rotationAxis[0].gameObject.SetActive(true);
                break;
            case visibleRotation.Y:
                rotationAxis[1].gameObject.SetActive(true);
                break;
            case visibleRotation.Z:
                rotationAxis[2].gameObject.SetActive(true);
                break;
            case visibleRotation.ALL:
                for (int i = 0; i < rotationAxis.Count; i++)
                {
                    rotationAxis[i].gameObject.SetActive(true);
                }
                break;
        }
    }
}