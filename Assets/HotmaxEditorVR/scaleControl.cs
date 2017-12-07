using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleControl : MonoBehaviour
{
    public objectSelect objSelect;
    public float scaleSize = .5f;

    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

    public Vector2 fingerPos;
    float rate = .1f;
    Vector3 growRate;

    private stateManager.editorModes _editorMode = stateManager.editorMode;


    private void Start()
    {
        growRate = new Vector3(rate, rate, rate);
        trackedObject = objSelect.hand2.GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);
    }

    private void OnEnable()
    {
        stateManager.editorModeEvent += updateEditorMode;
    }

    private void OnDisable()
    {
        stateManager.editorModeEvent -= updateEditorMode;

    }

    void updateEditorMode(stateManager.editorModes value)
    {
        _editorMode = value;
    }

    void Update()
    {
        if(_editorMode == stateManager.editorModes.openMenuMode)
        {
            //TODO: how is the touchPad controlling the Scale here?
            if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                if (device.GetAxis().y > .3f)
                {
                    transform.localScale += growRate;
                }
                if (device.GetAxis().y < -.3f)
                {
                    transform.localScale -= growRate;
                }
            }
        }
    }
}
