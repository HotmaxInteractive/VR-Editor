using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabSelect : MonoBehaviour
{
    //--local refs
    private stateManager _stateManagerMutatorRef;
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;

    //--save collided prop
    private GameObject collidedWithHand;
    private bool isCurrentlyColliding = false;

    //--hand for collider to follow
    private Transform grabberHand;

    private void Start()
    {
        _stateManagerMutatorRef = init._stateManagerMutatorRef;
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
        grabberHand = inputManager.hand2.gameObject.transform;
    }

    private void OnApplicationQuit()
    {
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
    }

    void Update()
    {
        transform.position = grabberHand.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<prop>())
        {
            if (other.gameObject.GetComponent<activeProp>())
            {
                if (_selectedObjectIsActive)
                {
                    _stateManagerMutatorRef.SET_EDITOR_MODE_FREE_GRAB();
                }
            }

            if (!isCurrentlyColliding && !_selectedObjectIsActive)
            {
                collidedWithHand = other.gameObject;
                _stateManagerMutatorRef.SET_OBJECT_COLLIDED_WITH_HAND(other.gameObject);
                isCurrentlyColliding = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<MonoBehaviour>() is prop)
        {
            if (collidedWithHand == other.gameObject)
            {
                _stateManagerMutatorRef.SET_OBJECT_COLLIDED_WITH_HAND(null);
                isCurrentlyColliding = false;
            }
        }
    }
}
