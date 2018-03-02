using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabSelect : MonoBehaviour
{
    //--local refs
    private stateManager _stateManagerMutatorRef;
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private GameObject _objectCollidedWithHand;
    private GameObject _selectedObject;
    //--hand for collider to follow
    private Transform _grabberHand;
    private SteamVR_TrackedController _trackedController2;

    private void Start()
    {
        _stateManagerMutatorRef = init._stateManagerMutatorRef;
        _grabberHand = inputManager.hand2.transform;
        _trackedController2 = inputManager.trackedController2;

        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
        stateManager.objectCollidedWithHandEvent += updateObjectCollidedWithHand;
        stateManager.selectedObjectEvent += updateSelectedObject;

        _trackedController2.TriggerClicked += triggerClicked;
        _trackedController2.TriggerUnclicked += triggerUnclicked;
    }

    private void OnApplicationQuit()
    {
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
        stateManager.objectCollidedWithHandEvent -= updateObjectCollidedWithHand;
        stateManager.selectedObjectEvent -= updateSelectedObject;

        _trackedController2.TriggerClicked -= triggerClicked;
        _trackedController2.TriggerUnclicked -= triggerUnclicked;
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
    }

    void updateObjectCollidedWithHand(GameObject value)
    {
        _objectCollidedWithHand = value;
    }

    void updateSelectedObject(GameObject value)
    {
        _selectedObject = value;
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        if (_objectCollidedWithHand != null)
        {
            if (_objectCollidedWithHand != _selectedObject)
            {
                _stateManagerMutatorRef.SET_SELECTED_OBJECT(_objectCollidedWithHand);
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
        transform.position = _grabberHand.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<prop>())
        {
            if (_objectCollidedWithHand == null)
            {
                _stateManagerMutatorRef.SET_OBJECT_COLLIDED_WITH_HAND(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_objectCollidedWithHand == other.gameObject)
        {
            _stateManagerMutatorRef.SET_OBJECT_COLLIDED_WITH_HAND(null);
        }
    }
}
