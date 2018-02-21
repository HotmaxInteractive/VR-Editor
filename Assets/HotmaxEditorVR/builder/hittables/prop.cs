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

    public void receiveHit(RaycastHit hit)
    {
        // selected object selection
        if (this.gameObject == _selectedObject)
        {
            _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(true);
        }

        // if new prop is selected
        else
        {
            _stateManagerMutatorRef.SET_SELECTED_OBJECT(hit.collider.gameObject);
            this.gameObject.AddComponent<activeProp>();
        }
    }
}

