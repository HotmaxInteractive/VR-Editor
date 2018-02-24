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
    private Transform initialParent;
    private GameObject lookAtRaycast;
    private GameObject stationaryLookAtRaycast;
    private GameObject propRotationHolder;
    private Vector3 lookPos;
    private Quaternion rotation;

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
        //on raycast + click trigger preserve this objects original rotation by parenting other object
        if (initialHit)
        {

            //do lookAt so the the perpendicular face to the locked axis faces the hit.point
            lookAtRaycast.transform.LookAt(hit.point, faceDirection);
            stationaryLookAtRaycast.transform.LookAt(hit.point, faceDirection);
            propRotationHolder.transform.LookAt(hit.point, faceDirection);

            transform.parent = propRotationHolder.transform;
            initialHit = false;
        }
        lookPos = hit.point - lookAtRaycast.transform.position;
        //lock the rotation to just one axis
        lockAxis = 0;
        //lookAtRaycast follows the hit.point with locked axis
        rotation = Quaternion.LookRotation(lookPos, faceDirection);

        lookAtRaycast.transform.rotation = Quaternion.Slerp(lookAtRaycast.transform.rotation, rotation, Time.deltaTime * 3);
        propRotationHolder.transform.rotation = Quaternion.Slerp(lookAtRaycast.transform.rotation, rotation, Time.deltaTime * 3);

    }

    private void Start()
    {
        propRotationHolder = Instantiate(Resources.Load("propRotationHolder", typeof(GameObject))) as GameObject;
        //set up the container for our object to rotate
        lookAtRaycast = Instantiate(Resources.Load("lookAtRaycast", typeof(GameObject))) as GameObject;
        //This should have a different apearance than the "lookAtRaycast object"
        stationaryLookAtRaycast = Instantiate(Resources.Load("lookAtRaycast", typeof(GameObject))) as GameObject;

    }

    private void OnEnable()
    {
       
        stateManager.rotationGizmoIsSelectedEvent += updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent += updateRotationModeEvent;
        inputManager.trackedController2.TriggerUnclicked += triggerUnclicked;

        //handle the visual representation of the rotation gizmo
        init.rotationGizmos.SetActive(true);
        init.rotationGizmos.transform.position = transform.position;
    }

    protected virtual void OnDisable()
    {
        stateManager.rotationGizmoIsSelectedEvent -= updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent -= updateRotationModeEvent;
        inputManager.trackedController2.TriggerUnclicked -= triggerUnclicked;

        //throws error when the app quits if we don't check if it is not null
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


            //save current parent
            initialHit = true;
        }
        else
        {
            //unparent object before destroying
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
                    init.rotationGizmos.transform.Find("xRotationGizmo").transform.Find("pie").gameObject.SetActive(true);
                    break;
                case stateManager.rotationModes.yRotationMode:
                    setRotationGizmoVisible(visibleRotation.Y);
                    init.rotationGizmos.transform.Find("yRotationGizmo").transform.Find("pie").gameObject.SetActive(true);
                    break;
                case stateManager.rotationModes.zRotationMode:
                    setRotationGizmoVisible(visibleRotation.Z);
                    init.rotationGizmos.transform.Find("zRotationGizmo").transform.Find("pie").gameObject.SetActive(true);
                    break;
            }
        }
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        init._stateManagerMutatorRef.SET_ROTATION_GIZMO_IS_SELECTED(false);
        init.rotationGizmos.transform.Find("xRotationGizmo").transform.Find("pie").gameObject.SetActive(false);
        init.rotationGizmos.transform.Find("yRotationGizmo").transform.Find("pie").gameObject.SetActive(false);
        init.rotationGizmos.transform.Find("zRotationGizmo").transform.Find("pie").gameObject.SetActive(false);
    }

    void setRotationGizmoVisible(visibleRotation visibleRotation)
    {
        Transform xGizmo = init.rotationGizmos.transform.Find("xRotationGizmo");
        Transform yGizmo = init.rotationGizmos.transform.Find("yRotationGizmo");
        Transform zGizmo = init.rotationGizmos.transform.Find("zRotationGizmo");

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