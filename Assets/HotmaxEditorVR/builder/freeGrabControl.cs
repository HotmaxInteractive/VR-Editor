using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeGrabControl : MonoBehaviour
{
    private stateManager _stateManagerMutatorRef;
    private Transform initialParent;

    private void OnEnable()
    {
        _stateManagerMutatorRef = init._stateManagerMutatorRef;
        inputManager.trackedController2.TriggerUnclicked += triggerUnclicked;
        initialParent = transform.parent;
        transform.parent = inputManager.hand2.transform;
    }

    private void OnDisable()
    {
        inputManager.trackedController2.TriggerUnclicked -= triggerUnclicked;
        transform.parent = init.props.transform;
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        //--now, in this version of freeGrab (telekinesis scrolling to hand), we can reset it back to Universal mode
        _stateManagerMutatorRef.SET_EDITOR_MODE_UNIVERSAL();
    }
}
