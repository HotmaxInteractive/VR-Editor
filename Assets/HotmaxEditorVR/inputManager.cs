using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
    public Valve.VR.InteractionSystem.Hand hand1;
    public Valve.VR.InteractionSystem.Hand hand2;

    public stateManager _stateManagerMutatorRef;

    SteamVR_TrackedObject trackedObject2;
    SteamVR_Controller.Device device2;

    public float scrollY;

    private stateManager.inputPadModes _inputPadMode = stateManager.inputPadMode;
    private stateManager.editorModes _editorMode = stateManager.editorMode;


    private void Awake()
    {
        stateManager.inputModeEvent += updateInputMode;
        stateManager.editorModeEvent += updateEditorMode;
    }

    protected virtual void OnApplicationQuit()
    {
        stateManager.inputModeEvent -= updateInputMode;
        stateManager.editorModeEvent -= updateEditorMode;
    }

    void updateInputMode(stateManager.inputPadModes value)
    {
        _inputPadMode = value;
        print(value);
    }

    void updateEditorMode(stateManager.editorModes value)
    {
        print(value);
    }

    void Start()
    {
        trackedObject2 = hand2.gameObject.GetComponent<SteamVR_TrackedObject>();

        //TODO: this shouldn't be an invoke, it should fire every second until a controller exists
        Invoke("setControllerIndecies", 1.2f);      
    }

    //TODO: this is a bug, if the controllers are turned on too late and if they arent being tracked...
    void setControllerIndecies()
    {
        if (trackedObject2 == null)
        {
            Invoke("setControllerIndecies", 1.2f);
            return;
        }
        //get the controller index
        uint hand1ControllerIndex = hand1.controller.index;
        uint hand2ControllerIndex = hand2.controller.index;

        hand1.gameObject.GetComponent<SteamVR_TrackedController>().controllerIndex = hand1ControllerIndex;
        hand2.gameObject.GetComponent<SteamVR_TrackedController>().controllerIndex = hand2ControllerIndex;

        // convert to uint to an int 
        hand1.gameObject.GetComponent<SteamVR_TrackedObject>().SetDeviceIndex((int)hand1ControllerIndex);
        hand2.gameObject.GetComponent<SteamVR_TrackedObject>().SetDeviceIndex((int)hand2ControllerIndex);

        device2 = SteamVR_Controller.Input((int)trackedObject2.index);
    }

    void Update()
    {
        //------------------------using the pad for a menu------------------------\\

        if(device2 != null)
        {
            if (device2.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                _stateManagerMutatorRef.SET_INPUT_MODE_TUNING();
            }

            if (device2.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                _stateManagerMutatorRef.SET_INPUT_MODE_QUADRANT();
            }

            if(_inputPadMode == stateManager.inputPadModes.tuningMode)
            {
                scrollY = device2.GetAxis().y;
            }

            if (_inputPadMode == stateManager.inputPadModes.quadrantMode)
            {
                if (device2.GetAxis().x != 0 || device2.GetAxis().y != 0)
                {
                    if (device2.GetAxis().x < 0 && device2.GetAxis().y > 0)
                    {
                        _stateManagerMutatorRef.SET_EDITOR_MODE_UNIVERSAL();
                    }
                    if (device2.GetAxis().x > 0 && device2.GetAxis().y > 0)
                    {
                        _stateManagerMutatorRef.SET_EDITOR_MODE_CLONE_DELETE();
                    }
                    if (device2.GetAxis().x < 0 && device2.GetAxis().y < 0)
                    {
                        _stateManagerMutatorRef.SET_EDITOR_MODE_OPEN_MENU();
                    }
                    if (device2.GetAxis().x > 0 && device2.GetAxis().y < 0)
                    {
                        _stateManagerMutatorRef.SET_EDITOR_MODE_OPEN_MENU();
                    }
                }
            }
        }
    }
}
