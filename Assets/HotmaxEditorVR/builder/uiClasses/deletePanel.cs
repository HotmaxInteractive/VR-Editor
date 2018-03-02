using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class deletePanel : MonoBehaviour, IHittable
{
    //--local refs
    private GameObject _selectedObject;
    private GameObject _deletedProps;

    public Transform target;

    private void Awake()
    {
        stateManager.selectedObjectEvent += updateSelectedObject;
    }

    private void Start()
    {
        _deletedProps = init.deletedProps;
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
        _selectedObject.transform.parent = _deletedProps.transform;
        _selectedObject.SetActive(false);
        gameObject.SetActive(false);
    }
}

