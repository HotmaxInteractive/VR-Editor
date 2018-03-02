using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleControl : MonoBehaviour
{
    //--local refs
    private GameObject _scaleController;
    private SteamVR_TrackedController _trackedController2;
    private GameObject _rotationGizmos;
    private GameObject _vrCamera;
    private Valve.VR.InteractionSystem.Hand _hand2;

    [SerializeField]
    private Vector3 growRate = new Vector3(.01f, .01f, .01f);

    private float initialYPos;
    private float currentYPos;
    [SerializeField]
    private float rayMoveUnit = .1f;

    private Vector3 rayOrigin;
    private Vector3 rayDirection;

    private void OnEnable()
    {
        _scaleController = init.scaleController;
        _trackedController2 = inputManager.trackedController2;
        _rotationGizmos = init.rotationGizmos;
        _vrCamera = init.vrCamera;
        _hand2 = inputManager.hand2;

        _trackedController2.TriggerClicked += triggerClicked;
        _scaleController.SetActive(true);
    }

    private void OnDisable()
    {
        _trackedController2.TriggerClicked -= triggerClicked;
        _scaleController.SetActive(false);
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        getInitialHitYPosition();
    }

    void Update()
    {
        setScaleControllerToFacePlayer();

        if (_trackedController2.triggerPressed)
        {
            scaleObject();
        }
    }

    void setScaleControllerToFacePlayer()
    {
        Transform scaleControllerHolder = _scaleController.transform.parent.transform;

        scaleControllerHolder.position = _rotationGizmos.transform.position;
        scaleControllerHolder.LookAt(_vrCamera.transform);
        scaleControllerHolder.eulerAngles = new Vector3(0, scaleControllerHolder.eulerAngles.y, 0);
    }

    void getInitialHitYPosition()
    {
        RaycastHit hit;
        rayOrigin = _hand2.gameObject.transform.position;
        rayDirection = _hand2.gameObject.transform.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        //--ray will just interact with the "Gizmo Layer"
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Gizmo Layer")))
        {
            if (hit.collider.gameObject == _scaleController)
            {
                initialYPos = hit.point.y;
            }
        }
    }

    void scaleObject()
    {
        RaycastHit hit;
        rayOrigin = _hand2.transform.position;
        rayDirection = _hand2.transform.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        //--ray will just interact with the "Gizmo Layer"
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Gizmo Layer")))
        {
            if (hit.collider.gameObject == _scaleController)
            {
                currentYPos = hit.point.y;

                if (currentYPos > initialYPos + rayMoveUnit)
                {
                    transform.localScale += growRate;
                    initialYPos = currentYPos;
                }

                //--guarding from going into negative scale
                if (transform.localScale.x > .01f)
                {
                    if (currentYPos < initialYPos - rayMoveUnit)
                    {
                        transform.localScale -= growRate;
                        initialYPos = currentYPos;
                    }
                }
            }
        }
    }
}
