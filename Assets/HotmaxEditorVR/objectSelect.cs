using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class objectSelect : MonoBehaviour
{
    //--laser stuff
    private Vector3 laserEndPosition;
    public LineRenderer laserLineRenderer;
    private float laserWidth = 0.01f;
    public float laserMaxLength = 5f;

    //--local refs
    private stateManager _stateManagerMutatorRef;
    private GameObject _selectedObject;

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
        //The helper laser for the selection tool. 
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.endWidth = laserWidth;

        inputManager.trackedController2.TriggerClicked += triggerClicked;
        inputManager.trackedController2.TriggerUnclicked += triggerUnclicked;
    }


    void triggerClicked(object sender, ClickedEventArgs e)
    {
        select(inputManager.hand2.gameObject.transform.position, inputManager.hand2.gameObject.transform.forward);
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(false);

        //TODO: Is this the correct position for this?
        _stateManagerMutatorRef.SET_ROTATION_GIZMO_IS_SELECTED(false);
    }

    void Update()
    {
        ShootLaserFromTargetPosition(inputManager.hand2.gameObject.transform.position, inputManager.hand2.gameObject.transform.forward, laserMaxLength);
    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        laserEndPosition = targetPosition + (length * direction);

        if (Physics.Raycast(ray, out raycastHit, length))
        {
            laserEndPosition = raycastHit.point;
        }

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, laserEndPosition);
    }


    void select(Vector3 targetPosition, Vector3 direction)
    {
        RaycastHit hit;
        Ray ray = new Ray(targetPosition, direction);
        if (Physics.Raycast(ray, out hit))
        {

            //validate that raycast is hitting something selectable
            if (hit.collider != null && !hit.collider.name.Contains("structure"))
            {

                // selected object selection
                if (hit.collider.gameObject == _selectedObject)
                {
                    _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(true);
                }

                // rotation gizmo selection
                // cant use gameobject rotational gizmo because it could be null
                // TODO: find better method to check NULL value
                else if (hit.collider.name.Contains("RotationGizmo"))
                {

                    _stateManagerMutatorRef.SET_ROTATION_GIZMO_IS_SELECTED(true);

                    if (hit.collider.name.Contains("xRotationGizmo"))
                    {
                        _stateManagerMutatorRef.SET_X_ROTATION_GIZMO_ACTIVE();
                    }
                    if (hit.collider.name.Contains("yRotationGizmo"))
                    {
                        _stateManagerMutatorRef.SET_Y_ROTATION_GIZMO_ACTIVE();
                    }
                    if (hit.collider.name.Contains("zRotationGizmo"))
                    {
                        _stateManagerMutatorRef.SET_Z_ROTATION_GIZMO_ACTIVE();
                    }
                }

                //default out to this
                else
                {
                    Debug.Log("default rayhit");
                    _stateManagerMutatorRef.SET_SELECTED_OBJECT(hit.collider.gameObject);
                    removeDecorators();
                    addDecorations(hit);
                }
            }

           
        }
    }

    void addDecorations(RaycastHit raycastHit)
    {
        GameObject hitObject = null;
        hitObject = raycastHit.collider.gameObject;

        //Add object controllers and reference this class
        hitObject.AddComponent<universalTransform>();
        hitObject.AddComponent<scaleControl>();
        hitObject.AddComponent<cloneControl>();

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
            Destroy(highlightedObject.GetComponent<universalTransform>());
            Destroy(highlightedObject.GetComponent<scaleControl>());
            Destroy(highlightedObject.GetComponent<cloneControl>());
            Destroy(outline);
        }
    }
}
