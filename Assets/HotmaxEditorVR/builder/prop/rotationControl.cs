using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationControl : MonoBehaviour
{
    //--local refs
    private bool _rotationGizmoIsSelected = stateManager.rotationGizmoIsSelected;
    private stateManager.rotationModes _rotationMode = stateManager.rotationMode;
    private Transform _hand2;
    private stateManager _stateManagerMutatorRef;
    private GameObject _rotationGizmos;
    private GameObject _props;
    private SteamVR_TrackedController _trackedController2;

    //--vars used during rotation
    private bool initialHit = false;
    private GameObject rotationGage;
    private GameObject stationaryRotationGage;
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
        ALL
    }

    private void Start()
    {
        //holder for prop during rotation
        propRotationHolder = Instantiate(Resources.Load("propRotationHolder", typeof(GameObject))) as GameObject;
        //--rotation offset gages
        rotationGage = Instantiate(Resources.Load("rotationGage", typeof(GameObject))) as GameObject;
        stationaryRotationGage = Instantiate(Resources.Load("rotationGage", typeof(GameObject))) as GameObject;
    }

    private void OnEnable()
    {
        _hand2 = inputManager.hand2.transform;
        _stateManagerMutatorRef = init._stateManagerMutatorRef;
        _rotationGizmos = init.rotationGizmos;
        _props = init.props;
        _trackedController2 = inputManager.trackedController2;

        stateManager.rotationGizmoIsSelectedEvent += updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent += updateRotationModeEvent;
        _trackedController2.TriggerUnclicked += triggerUnclicked;

        _rotationGizmos.SetActive(true);
        _rotationGizmos.transform.position = transform.position;
    }

    protected virtual void OnDisable()
    {
        stateManager.rotationGizmoIsSelectedEvent -= updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent -= updateRotationModeEvent;
        _trackedController2.TriggerUnclicked -= triggerUnclicked;

        if (_rotationGizmos != null)
        {
            _rotationGizmos.SetActive(false);
        }
    }

    void updateRotationGizmoIsSelected(bool value)
    {
        _rotationGizmoIsSelected = value;
        setRotationGizmoVisible(visibleRotation.ALL);

        if (_rotationGizmoIsSelected)
        {
            setUpRotationGages();
            propRotationHolder.transform.position = transform.position;

            initialHit = true;
        }
        else
        {
            //re-parent back in props
            transform.parent = _props.transform;
            rotationGage.SetActive(false);
            stationaryRotationGage.SetActive(false);
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
        _stateManagerMutatorRef.SET_ROTATION_GIZMO_IS_SELECTED(false);
        setAxisColliderActive("x", false);
        setAxisColliderActive("y", false);
        setAxisColliderActive("z", false);
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(_hand2.position, _hand2.forward);
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
            rotationGage.transform.LookAt(hit.point, faceDirection);
            stationaryRotationGage.transform.LookAt(hit.point, faceDirection);
            propRotationHolder.transform.LookAt(hit.point, faceDirection);

            transform.parent = propRotationHolder.transform;
            initialHit = false;
        }

        lookPos = hit.point - rotationGage.transform.position;
        //locks the rotation to just one axis
        lockAxis = 0;

        Quaternion rotateDirection = Quaternion.LookRotation(lookPos, faceDirection);

        rotationGage.transform.rotation = Quaternion.Slerp(rotationGage.transform.rotation, rotateDirection, Time.deltaTime * 3);
        propRotationHolder.transform.rotation = Quaternion.Slerp(propRotationHolder.transform.rotation, rotateDirection, Time.deltaTime * 3);
    }

    void setRotationGizmoVisible(visibleRotation visibleRotation)
    {
        Transform xGizmo = _rotationGizmos.transform.Find("x" + rotationGizmo);
        Transform yGizmo = _rotationGizmos.transform.Find("y" + rotationGizmo);
        Transform zGizmo = _rotationGizmos.transform.Find("z" + rotationGizmo);

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
                setAxisColliderActive("x", true);
                break;
            case visibleRotation.Y:
                rotationAxis[1].gameObject.SetActive(true);
                setAxisColliderActive("y", true);
                break;
            case visibleRotation.Z:
                rotationAxis[2].gameObject.SetActive(true);
                setAxisColliderActive("z", true);
                break;
            case visibleRotation.ALL:
                for (int i = 0; i < rotationAxis.Count; i++)
                {
                    rotationAxis[i].gameObject.SetActive(true);
                }
                break;
        }
    }

    void setAxisColliderActive(string axis, bool active)
    {
        _rotationGizmos.transform.Find(axis + rotationGizmo).transform.Find(rotationAxisCollider).gameObject.SetActive(active);
    }

    void setUpRotationGages()
    {
        rotationGage.SetActive(true);
        stationaryRotationGage.SetActive(true);
        rotationGage.transform.position = transform.position;
        stationaryRotationGage.transform.position = transform.position;
    }
}