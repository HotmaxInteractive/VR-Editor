using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleControl : MonoBehaviour
{
    public float scaleSize = .5f;
    private float rate = .1f;
    private Vector3 growRate;

    private stateManager.editorModes _editorMode = stateManager.editorMode;

    private void Start()
    {
        growRate = new Vector3(rate, rate, rate);
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
        if(_editorMode == stateManager.editorModes.spawnMenuMode)
        {
            //TODO: how is the touchPad controlling the Scale here?
            if (inputManager.selectorHand.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                if (inputManager.selectorHand.GetAxis().y > .3f)
                {
                    transform.localScale += growRate;
                }
                if (inputManager.selectorHand.GetAxis().y < -.3f)
                {
                    transform.localScale -= growRate;
                }
            }
        }
    }
}
