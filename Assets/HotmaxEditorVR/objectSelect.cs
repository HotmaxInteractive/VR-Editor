using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class objectSelect : MonoBehaviour
{
    public Valve.VR.InteractionSystem.Hand hand1;
    public Valve.VR.InteractionSystem.Hand hand2;

    [HideInInspector]
    public SteamVR_TrackedController trackedController1;
    [HideInInspector]
    public SteamVR_TrackedController trackedController2;

    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

    public Vector3 endPosition;
    public inputManager inputManagerRef;

    public LineRenderer laserLineRenderer;
    float laserWidth = 0.01f;
    public float laserMaxLength = 5f;

    stateManager _stateManagerMutatorRef;

    GameObject _selectedObject;

    private void Awake()
    {
        _stateManagerMutatorRef = GameObject.FindObjectOfType(typeof(stateManager)) as stateManager;
        stateManager.selectedObjectEvent += updateSelectedObject;
    }

    protected virtual void OnApplicationQuit()
    {
        stateManager.selectedObjectEvent -= updateSelectedObject;
    }

    void updateSelectedObject(GameObject value)
    {
        _selectedObject = value;
    }



    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.endWidth = laserWidth;

        trackedController1 = hand1.gameObject.GetComponent<SteamVR_TrackedController>();
        trackedController2 = hand2.gameObject.GetComponent<SteamVR_TrackedController>();

        trackedController2.TriggerClicked += triggerClicked;
        trackedController2.TriggerUnclicked += triggerUnclicked;

        Invoke("getAndSetControllerIndecies", 1.2f);

        trackedObject = hand2.GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);
    }

    private void OnDisable()
    {
        trackedController2.TriggerClicked -= triggerClicked;
        trackedController2.TriggerUnclicked -= triggerUnclicked;

    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        select(hand2.gameObject.transform.position, hand2.gameObject.transform.forward);
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(false);
    }


    //TODO: this is a bug, if the controllers are turned on too late and if they arent being tracked...
    void getAndSetControllerIndecies()
    {
        //get the controller index
        uint hand1ControllerIndex = hand1.controller.index;
        uint hand2ControllerIndex = hand2.controller.index;

        hand1.gameObject.GetComponent<SteamVR_TrackedController>().controllerIndex = hand1ControllerIndex;
        hand2.gameObject.GetComponent<SteamVR_TrackedController>().controllerIndex = hand2ControllerIndex;

        // convert to uint to an int 
        hand1.gameObject.GetComponent<SteamVR_TrackedObject>().SetDeviceIndex((int)hand1ControllerIndex);
        hand2.gameObject.GetComponent<SteamVR_TrackedObject>().SetDeviceIndex((int)hand2ControllerIndex);
    }

    void Update()
    {
        ShootLaserFromTargetPosition(hand2.gameObject.transform.position, hand2.gameObject.transform.forward, laserMaxLength);
    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        endPosition = targetPosition + (length * direction);

        if (Physics.Raycast(ray, out raycastHit, length))
        {
            endPosition = raycastHit.point;
        }

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, endPosition);
    }


    void select(Vector3 targetPosition, Vector3 direction)
    {
        //TODO: write this so the event listener is inside this method so I can check in the update as well
        RaycastHit hit;
        Ray ray = new Ray(targetPosition, direction);
        if (Physics.Raycast(ray, out hit))
        {

            //check to see if the raycast is hitting a game object
            if (hit.collider != null && !hit.collider.name.Contains("structure"))
            {
                // if the selected object is different from the currently selected object
                if(hit.collider.gameObject == _selectedObject)
                {
                    _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(true);
                }
                else
                {
                    _stateManagerMutatorRef.SET_SELECTED_OBJECT(hit.collider.gameObject);
                    removeDecorators();
                    addDecorations(hit);
                }
            }

            if (hit.collider.name.Contains("spawnSlot"))
            {
               
                if (hit.collider.name.Contains("(1)"))
                {
                    GameObject spawn = Instantiate(Resources.Load("(1)", typeof(GameObject))) as GameObject;
                    spawn.transform.position = hit.collider.transform.position;
                    spawn.transform.rotation = hit.collider.transform.rotation;
                }
                if (hit.collider.name.Contains("(2)"))
                {
                    GameObject spawn = Instantiate(Resources.Load("(2)", typeof(GameObject))) as GameObject;
                    spawn.transform.position = hit.collider.transform.position;
                    spawn.transform.rotation = hit.collider.transform.rotation;
                }
                if (hit.collider.name.Contains("(3)"))
                {
                    GameObject spawn = Instantiate(Resources.Load("(3)", typeof(GameObject))) as GameObject;
                    spawn.transform.position = hit.collider.transform.position;
                    spawn.transform.rotation = hit.collider.transform.rotation;
                }
                if (hit.collider.name.Contains("(4)"))
                {
                    GameObject spawn = Instantiate(Resources.Load("(4)", typeof(GameObject))) as GameObject;
                    spawn.transform.position = hit.collider.transform.position;
                    spawn.transform.rotation = hit.collider.transform.rotation;
                }
            }
        }
    }

     void addDecorations(RaycastHit raycastHit)
    {
        GameObject hitObject = null;
        hitObject = raycastHit.collider.gameObject;

        //Add object controllers and reference this class
        hitObject.AddComponent<positionControl>();
        hitObject.GetComponent<positionControl>().objSelect = this;

        hitObject.AddComponent<scaleControl>();
        hitObject.GetComponent<scaleControl>().objSelect = this;

        hitObject.AddComponent<cloneControl>();
        hitObject.GetComponent<cloneControl>().objSelect = this;

        hitObject.AddComponent<editStateController>();
        hitObject.GetComponent<editStateController>().objSelect = this;

        //Add the "selectable outline"
        hitObject.AddComponent<cakeslice.Outline>();
    }


    void removeDecorators()
    {
        //Clean up old highlighted object before adding new stuff
        cakeslice.Outline outline = (cakeslice.Outline)FindObjectOfType(typeof(cakeslice.Outline));
        if (outline)
        {
            GameObject highlightedObject = outline.transform.gameObject;
            Destroy(highlightedObject.GetComponent<positionControl>());
            Destroy(highlightedObject.GetComponent<scaleControl>());
            Destroy(highlightedObject.GetComponent<cloneControl>());
            Destroy(highlightedObject.GetComponent<editStateController>());
            Destroy(outline);
        }
    }
}
