using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class deletePanel : MonoBehaviour, IHittable
{
    //--local refs
    private stateManager _stateManagerMutatorRef;
    private GameObject _selectedObject;
    private stateManager.editorModes _editorMode = stateManager.editorMode;


    private void Awake()
    {
        _stateManagerMutatorRef = GameObject.FindObjectOfType(typeof(stateManager)) as stateManager;
        stateManager.selectedObjectEvent += updateSelectedObject;

    }

    private void OnApplicationQuit()
    {
        stateManager.selectedObjectEvent -= updateSelectedObject;
    }

    void updateSelectedObject(GameObject value)
    {
        _selectedObject = value;
    }

    public void receiveHit(RaycastHit hit)
    {
        _selectedObject.transform.parent = init.deletedProps.transform;
        init.deletePanel.SetActive(false);
        _selectedObject.SetActive(false);
    }
}

