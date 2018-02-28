using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
    //--local refs
    private bool _rotationGizmoIsSelected = stateManager.rotationGizmoIsSelected;

    // -- controller refs
    //TODO: make these private in the future if nothing is grabbing it from the outside
    public static Valve.VR.InteractionSystem.Hand hand1;
    public static Valve.VR.InteractionSystem.Hand hand2;
    public static SteamVR_TrackedController trackedController1;
    public static SteamVR_TrackedController trackedController2;
    private SteamVR_TrackedObject trackedObject2;
    public static SteamVR_Controller.Device selectorHand;

    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;    


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
        selectorHand = SteamVR_Controller.Input((int)trackedObject2.index);

        //This method depends on another steamVR method which fires 1s after the app starts.
        //The steamVR method finds the hardware indecies of the controllers
        Invoke("setControllerIndecies", 1.2f);
    }

    //TODO: this should probably be an event that broadcasts in steamVR Hand, but this seems to be working for now
    //------and edge case might be if only one controller is on
    void setControllerIndecies()
    {
        if (hand2.controller == null)
        {
            Invoke("setControllerIndecies", 1f);
            print("trying to find a hardware for the controller (Hand.controller)");
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

        selectorHand = SteamVR_Controller.Input((int)trackedObject2.index);
    }
}
