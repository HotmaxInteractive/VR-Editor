﻿using System.Collections;
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
    private Transform initialPropParent;

    //--local refs
    GameObject _objectCollidedWithHand;

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
        stateManager.objectCollidedWithHandEvent += updateObjectCollidedWithHand;

    }

    private void OnApplicationQuit()
    {
        inputManager.trackedController2.TriggerClicked -= triggerClicked;
        inputManager.trackedController2.TriggerUnclicked -= triggerUnclicked;
        stateManager.objectCollidedWithHandEvent -= updateObjectCollidedWithHand;

    }


    void triggerClicked(object sender, ClickedEventArgs e)
    {
        //put if else here, if colliding with IHittable object for freegrab
        if(_objectCollidedWithHand == null)
        {
            select(inputManager.hand2.gameObject.transform.position, inputManager.hand2.gameObject.transform.forward);
        }
        else
        {
            //probably do other stuff to the prop too
            initialPropParent = _objectCollidedWithHand.transform.parent;
            _objectCollidedWithHand.transform.parent = inputManager.hand2.transform;
        }
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        //TODO: move this to somewhere where it makes more sense
        _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(false);

        if(_objectCollidedWithHand !=  null)
        {
            _objectCollidedWithHand.transform.parent = initialPropParent;
            _objectCollidedWithHand = null;
        }
    }

    void updateObjectCollidedWithHand(GameObject value)
    {
        _objectCollidedWithHand = value;
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
                //have hit instance fire receiveHit function
                hit.collider.gameObject.GetComponent<IHittable>().receiveHit(hit);
            }   
        }
    }
}




public interface IHittable
{
    void receiveHit(RaycastHit hit);
}
