using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportPlayerAndRotate : MonoBehaviour
{
    public Transform teleportToSpot;
    private GameObject _selectedObject;
    private bool _rotationGizmoIsSelected;

    private void Start()
    {
        stateManager.selectedObjectEvent += updateSelectedObject;
        stateManager.rotationGizmoIsSelectedEvent += updateRotationGizmoIsSelected;
    }

    private void OnApplicationQuit()
    {
        stateManager.selectedObjectEvent -= updateSelectedObject;
        stateManager.rotationGizmoIsSelectedEvent += updateRotationGizmoIsSelected;
    }

    void updateSelectedObject(GameObject value)
    {
        if(value.name.Contains("pad"))
        {
            _selectedObject = value;
        }
    }

    void updateRotationGizmoIsSelected(bool value)
    {
        _rotationGizmoIsSelected = value;
    }

    public void teleportAndRotate()
    {
        init.player.transform.position = teleportToSpot.position;
        init.player.transform.eulerAngles = new Vector3(0, teleportToSpot.eulerAngles.y, 0);
    }

    private void Update()
    {
        if(!_rotationGizmoIsSelected)
        {
            return;
        }
        else
        {
            if (_selectedObject.name.Contains("pad"))
            {
                init.player.transform.eulerAngles = new Vector3(0, _selectedObject.transform.eulerAngles.y, 0);
            }
        }
    }
}
