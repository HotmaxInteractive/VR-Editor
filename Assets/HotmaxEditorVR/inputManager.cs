using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
    public Valve.VR.InteractionSystem.Hand hand1;
    public Valve.VR.InteractionSystem.Hand hand2;

    stateManager _stateManagerMutatorRef;
    SteamVR_TrackedObject trackedObject2;
    SteamVR_Controller.Device device2;

    public float scrollY;

    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private stateManager.editorModes _editorMode = stateManager.editorMode;
    


    private void Awake()
    {
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
        //------------------------using the pad------------------------\\

        if(device2 != null)
        {
            if(_selectedObjectIsActive == true)
            {
                scrollY = device2.GetAxis().y;
            }

            //using the quadrants
            if (_selectedObjectIsActive == false)
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
