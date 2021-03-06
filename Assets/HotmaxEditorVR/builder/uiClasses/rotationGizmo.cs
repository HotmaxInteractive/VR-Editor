﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class rotationGizmo : MonoBehaviour, IHittable
{
    public UnityEvent gizmoRotationEvent;

    //--local refs
    private stateManager _stateManagerMutatorRef;

    private void Start()
    {
        _stateManagerMutatorRef = init._stateManagerMutatorRef;
    }
    
    public void receiveHit(RaycastHit hit)
    {
        _stateManagerMutatorRef.SET_ROTATION_GIZMO_IS_SELECTED(true);
        gizmoRotationEvent.Invoke();
    }
}
