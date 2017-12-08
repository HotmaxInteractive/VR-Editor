using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
    // -- controller refs
    public static Valve.VR.InteractionSystem.Hand hand1;
    public static Valve.VR.InteractionSystem.Hand hand2;
    public static SteamVR_TrackedController trackedController1;
    public static SteamVR_TrackedController trackedController2;
    private SteamVR_TrackedObject trackedObject2;
    public static SteamVR_Controller.Device device2;


    private stateManager _stateManagerMutatorRef;
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private stateManager.editorModes _editorMode = stateManager.editorMode;
    


    private void Awake()
    {
        //Match up the hardware index with the "steamVR Hand2"
        //So the Hand2 is always the starting rightmost hand reletive to the HMD
        //Not sure why steamVR's select hand option doesn't do this, let me know if you figure it out
        hand1 = GameObject.Find("Hand1").GetComponent<Valve.VR.InteractionSystem.Hand>();
        hand2 = GameObject.Find("Hand2").GetComponent<Valve.VR.InteractionSystem.Hand>();
        trackedController1 = hand1.gameObject.GetComponent<SteamVR_TrackedController>();
        trackedController2 = hand2.gameObject.GetComponent<SteamVR_TrackedController>();
        trackedObject2 = hand2.GetComponent<SteamVR_TrackedObject>();
        device2 = SteamVR_Controller.Input((int)trackedObject2.index);


        Invoke("setControllerIndecies", 1.2f);

        _stateManagerMutatorRef = GameObject.FindObjectOfType(typeof(stateManager)) as stateManager;
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
        stateManager.editorModeEvent += updateEditorMode;
    }

    protected virtual void OnApplicationQuit()
    {
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
        stateManager.editorModeEvent -= updateEditorMode;

    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
    }

    void updateEditorMode(stateManager.editorModes value)
    {
        print(value);
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
        //------------------------using the pad------------------------\\

        if(device2 != null)
        {
            if(_selectedObjectIsActive)
            {
                return;
            }
            else
            {
                //TODO: move to menu handler
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
