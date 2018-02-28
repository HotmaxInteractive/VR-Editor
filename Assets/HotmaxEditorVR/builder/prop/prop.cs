using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prop : MonoBehaviour, IHittable
{
    //--local refs
    private stateManager _stateManagerMutatorRef;
    private GameObject _selectedObject;

    private void Awake()
    {
        _stateManagerMutatorRef = FindObjectOfType(typeof(stateManager)) as stateManager;
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

    //--This is the raycast way to select this object
    public void receiveHit(RaycastHit hit)
    {
        // selected object selection
        if (gameObject == _selectedObject)
        {
            _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(true);
        }

        // if new prop is selected
        else
        {
            _stateManagerMutatorRef.SET_SELECTED_OBJECT(hit.collider.gameObject);
            gameObject.AddComponent<activeProp>();
        }
    }
}

