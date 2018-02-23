using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabSelect : MonoBehaviour
{
    //--local refs
    private stateManager _stateManagerMutatorRef;
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private GameObject _objectCollidedWithHand;

    //--save collided prop
    private GameObject collidedWithHand;
    private bool isCurrentlyColliding = false;

    //--hand for collider to follow
    private Transform grabberHand;

    private Transform initialPropParent;

    private void Start()
    {
        _stateManagerMutatorRef = init._stateManagerMutatorRef;

        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
        inputManager.trackedController2.TriggerClicked += triggerClicked;
        inputManager.trackedController2.TriggerUnclicked += triggerUnclicked;
        stateManager.objectCollidedWithHandEvent += updateObjectCollidedWithHand;

        grabberHand = inputManager.hand2.gameObject.transform;
    }

    private void OnApplicationQuit()
    {
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
        inputManager.trackedController2.TriggerClicked -= triggerClicked;
        inputManager.trackedController2.TriggerUnclicked -= triggerUnclicked;
        stateManager.objectCollidedWithHandEvent -= updateObjectCollidedWithHand;
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
    }

    void updateObjectCollidedWithHand(GameObject value)
    {
        _objectCollidedWithHand = value;
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        if (_objectCollidedWithHand != null)
        {
            _stateManagerMutatorRef.SET_SELECTED_OBJECT(_objectCollidedWithHand);
            if(!_objectCollidedWithHand.GetComponent<activeProp>())
            {
                _objectCollidedWithHand.AddComponent<activeProp>();
            }
            _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(true);
        }
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        if (_objectCollidedWithHand != null)
        {
            _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(false);
        }
    }

    void Update()
    {
        transform.position = grabberHand.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<prop>())
        {
            //--isCurrentlyColliding fires once
            if (!isCurrentlyColliding)
            {
                setObjectCollidedWithHand(other.gameObject, true);
            }
            ////--handle telekinesis
            //if (other.gameObject.GetComponent<activeProp>())
            //{
            //    if (_selectedObjectIsActive)
            //    {
            //        _stateManagerMutatorRef.SET_OBJECT_COLLIDED_WITH_HAND(other.gameObject);
            //        //_stateManagerMutatorRef.SET_EDITOR_MODE_FREE_GRAB();
            //    }
            //}

            ////--handle new prop grabbed
            //if (!isCurrentlyColliding && !_selectedObjectIsActive)
            //{
            //    collidedWithHand = other.gameObject;
            //    _stateManagerMutatorRef.SET_OBJECT_COLLIDED_WITH_HAND(other.gameObject);
            //    isCurrentlyColliding = true;
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (collidedWithHand == other.gameObject)
        {
            setObjectCollidedWithHand(null, false);
        }       
    }

    void setObjectCollidedWithHand(GameObject prop, bool currentlyColliding)
    {
        _stateManagerMutatorRef.SET_OBJECT_COLLIDED_WITH_HAND(prop);
        collidedWithHand = prop;
        isCurrentlyColliding = currentlyColliding;
    }
}
