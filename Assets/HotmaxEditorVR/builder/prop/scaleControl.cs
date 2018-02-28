using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleControl : MonoBehaviour
{
    private Vector3 growRate = new Vector3(.01f, .01f, .01f);

    private float initialYPos;
    private float currentYPos;

    private Vector3 rayOrigin;
    private Vector3 rayDirection;

    private void OnEnable()
    {
        inputManager.trackedController2.TriggerClicked += triggerClicked;
        init.scaleController.SetActive(true);
    }

    private void OnDisable()
    {
        inputManager.trackedController2.TriggerClicked -= triggerClicked;
        init.scaleController.SetActive(false);
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        getInitialHitYPosition();
    }

    void Update()
    {
        setScaleControllerToFacePlayer();

        if (inputManager.trackedController2.triggerPressed)
        {
            scaleObject();
        }
    }

    void setScaleControllerToFacePlayer()
    {
        Transform scaleControllerHolder = init.scaleController.transform.parent.transform;

        scaleControllerHolder.position = init.rotationGizmos.transform.position;
        scaleControllerHolder.LookAt(init.vrCamera.transform);
        scaleControllerHolder.eulerAngles = new Vector3(0, scaleControllerHolder.eulerAngles.y, 0);
    }

    void getInitialHitYPosition()
    {
        RaycastHit hit;
        rayOrigin = inputManager.hand2.gameObject.transform.position;
        rayDirection = inputManager.hand2.gameObject.transform.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        //--ray will just interact with the "Gizmo Layer"
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Gizmo Layer")))
        {
            if (hit.collider.gameObject == init.scaleController)
            {
                initialYPos = hit.point.y;
            }
        }
    }

    void scaleObject()
    {
        RaycastHit hit;
        rayOrigin = inputManager.hand2.gameObject.transform.position;
        rayDirection = inputManager.hand2.gameObject.transform.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        //--ray will just interact with the "Gizmo Layer"
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Gizmo Layer")))
        {
            if (hit.collider.gameObject == init.scaleController)
            {
                currentYPos = hit.point.y;

                if (currentYPos > initialYPos + .1f)
                {
                    transform.localScale += growRate;
                    initialYPos = currentYPos;
                }

                //--gaurding from going into negative scale
                if (transform.localScale.x > .01f)
                {
                    if (currentYPos < initialYPos - .1f)
                    {
                        transform.localScale -= growRate;
                        initialYPos = currentYPos;
                    }
                }
            }
        }
    }
}
