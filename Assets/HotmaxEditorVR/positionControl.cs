using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionControl : MonoBehaviour
{
    public objectSelect objSelect;
    editStateController stateController;

    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

    public Vector2 fingerPos;
    float rate = .1f;
    Vector3 slideRate;

    private void Start()
    {
        stateController = GetComponent<editStateController>();
        slideRate = new Vector3(rate, rate, rate);

        trackedObject = objSelect.hand2.GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);
    }

    void Update()
    {
        if (objSelect.trackedController2.triggerPressed)
        {
            transform.position = objSelect.endPosition;
        }

        //TODO: how is the touchPad controlling the Scale here?
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            //move toward the hit position
            if (device.GetAxis().y > .3f)
            {
                transform.position = Vector3.MoveTowards(transform.position, objSelect.endPosition, rate);
            }

            //move toward the controller
            if (device.GetAxis().y < -.3f)
            {
                transform.position = Vector3.MoveTowards(transform.position, objSelect.hand2.transform.position, rate);
            }
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            stateController.enabled = false;
            //Get initial finger position
            fingerPos.y = device.GetAxis().y;
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            stateController.enabled = true;
        }
    }
}
