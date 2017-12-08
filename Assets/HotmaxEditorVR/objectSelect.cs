using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class objectSelect : MonoBehaviour
{
    //--laser stuff
    private Vector3 laserEndPosition;
    public LineRenderer laserLineRenderer;
    [SerializeField]
    private float laserWidth = 0.01f;
    [SerializeField]
    private float laserMaxLength = 5f;

    
    private stateManager _stateManagerMutatorRef;

    private void Awake()
    {
        _stateManagerMutatorRef = GameObject.FindObjectOfType(typeof(stateManager)) as stateManager;
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


    //TODO: move this to somewhere where it makes more sense
    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(false);
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
            if(hit.collider.gameObject.GetComponent<MonoBehaviour>() is IHittable)
            {
                hit.collider.gameObject.GetComponent<IHittable>().receiveHit(hit);
            }   
        }
    }
}




public interface IHittable
{
    void receiveHit(RaycastHit hit);
}
