using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleControl : MonoBehaviour
{
    public objectSelect objSelect;
    editStateController stateController;
    public float scaleSize = .5f;

    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

    public Vector2 fingerPos;
    float rate = .1f;
    Vector3 growRate;

    private void Start()
    {
        growRate = new Vector3(rate, rate, rate);
        stateController = GetComponent<editStateController>();

        trackedObject = objSelect.hand2.GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);
    }

    void Update()
    {
        //TODO: how is the touchPad controlling the Scale here?
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if(device.GetAxis().y > .3f)
            {
                transform.localScale += growRate;
            }
            if (device.GetAxis().y < -.3f)
            {
                transform.localScale -= growRate;
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
