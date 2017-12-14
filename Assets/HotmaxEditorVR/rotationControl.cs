using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationControl : MonoBehaviour
{
    //--local refs
    private bool _rotationGizmoIsSelected = stateManager.rotationGizmoIsSelected;
    private stateManager.rotationModes _rotationMode = stateManager.rotationMode;

    private List<Transform> transforms = new List<Transform>();

    bool initialHit = false;
    private Transform initialParent;
    private Vector3 targetPostition;
    private GameObject lookAtRaycast;

    Vector3 initialRotation;

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
                Vector3 lookPos;
                Quaternion rotation;

                if (_rotationGizmoIsSelected)
                {
                    switch (_rotationMode)
                    {
                        case stateManager.rotationModes.xRotationMode:
                            //preserve this objects original rotation by parenting into an object that is sdoing the lookAT
                            if (initialHit)
                            {
                                //do lookAt so the lookAtRaycast objects right side is facing hit.point
                                lookAtRaycast.transform.LookAt(hit.point, Vector3.right);
                                transform.parent = lookAtRaycast.transform;
                                initialHit = false;
                            }
                            lookPos = hit.point - lookAtRaycast.transform.position;
                            //lock the x rotation to just the x axis
                            lookPos.x = 0;
                            //lookAtRaycast's right side needs to follow the hit.point on a locked Y axis
                            rotation = Quaternion.LookRotation(lookPos, Vector3.right);
                            lookAtRaycast.transform.rotation = Quaternion.Slerp(lookAtRaycast.transform.rotation, rotation, Time.deltaTime * 3);
                            break;

                        case stateManager.rotationModes.yRotationMode:
                            if (initialHit)
                            {
                                lookAtRaycast.transform.LookAt(hit.point, Vector3.up);
                                transform.parent = lookAtRaycast.transform;
                                initialHit = false;
                            }

                            lookPos = hit.point - lookAtRaycast.transform.position;
                            lookPos.y = 0;
                            rotation = Quaternion.LookRotation(lookPos, Vector3.up);
                            lookAtRaycast.transform.rotation = Quaternion.Slerp(lookAtRaycast.transform.rotation, rotation, Time.deltaTime * 3);
                            break;

                        case stateManager.rotationModes.zRotationMode:
                            if (initialHit)
                            {
                                lookAtRaycast.transform.LookAt(hit.point, Vector3.forward);
                                transform.parent = lookAtRaycast.transform;
                                initialHit = false;
                            }

                            lookPos = hit.point - lookAtRaycast.transform.position;
                            lookPos.z = 0;
                            rotation = Quaternion.LookRotation(lookPos, Vector3.forward);
                            lookAtRaycast.transform.rotation = Quaternion.Slerp(lookAtRaycast.transform.rotation, rotation, Time.deltaTime * 3);
                            break;
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        stateManager.rotationGizmoIsSelectedEvent += updateRotationGizmoIsSelected;
        stateManager.rotationModeEvent += updateRotationModeEvent;

        //handle the visual representation of the rotation gizmo
        init.rotationGizmos.SetActive(true);
        init.rotationGizmos.transform.position = transform.position;

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
        setRotationGizmoVisible(visibleRotation.ALL);

        if(_rotationGizmoIsSelected)
        {
            lookAtRaycast = GameObject.CreatePrimitive(PrimitiveType.Cube);

            lookAtRaycast.GetComponent<MeshRenderer>().enabled = false;
            lookAtRaycast.GetComponent<BoxCollider>().enabled = false;

            lookAtRaycast.transform.position = transform.position;

            initialParent = transform.parent;
            initialHit = true;
        }
        else
        {
            transform.parent = initialParent;
            Destroy(lookAtRaycast);
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
