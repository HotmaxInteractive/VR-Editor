using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleControl : MonoBehaviour
{
    public float scaleSize = .5f;
    private float rate = .01f;
    private Vector3 growRate;

    private float initialYPos;
    private float currentYPos;

    private Vector3 rayOrigin;
    private Vector3 rayDirection;

    private void Start()
    {
        growRate = new Vector3(rate, rate, rate);
    }

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

    void Update()
    {
        setScaleControllerToFacePlayer();

        if (inputManager.trackedController2.triggerPressed)
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

    void setScaleControllerToFacePlayer()
    {
        Transform scaleControllerHolder = init.scaleController.transform.parent.transform;

        scaleControllerHolder.position = init.rotationGizmos.transform.position;
        scaleControllerHolder.LookAt(init.vrCamera.transform);
        scaleControllerHolder.eulerAngles = new Vector3(0, scaleControllerHolder.eulerAngles.y, 0);
    }
}
