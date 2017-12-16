using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class deletePanel : MonoBehaviour, IHittable
{
    //--local refs
    private GameObject _selectedObject;

    public Transform target;

    private void Awake()
    {
        stateManager.selectedObjectEvent += updateSelectedObject;
    }

    private void OnApplicationQuit()
    {
        stateManager.selectedObjectEvent -= updateSelectedObject;
    }

    void Update()
    {
        transform.LookAt(target);
    }

    void updateSelectedObject(GameObject value)
    {
        _selectedObject = value;
    }

    //TODO: flesh out what to do with deleted props
    public void receiveHit(RaycastHit hit)
    {
        _selectedObject.transform.parent = init.deletedProps.transform;
        _selectedObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}

