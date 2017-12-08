using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloneControl : MonoBehaviour
{
    private bool cloneMode = false;

    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private GameObject _selectedObject;
    private stateManager.editorModes _editorMode = stateManager.editorMode;

    private void Start()
    {
        stateManager.editorModeEvent += updateEditorMode;
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
        stateManager.selectedObjectEvent += updateSelectedObjectEvent;

        cloneSelectedObject();
    }

    protected virtual void OnApplicationQuit()
    {
        stateManager.editorModeEvent -= updateEditorMode;
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
    }

    void updateEditorMode(stateManager.editorModes value)
    {
        _editorMode = value;
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
        if (_selectedObjectIsActive)
        {
            cloneSelectedObject();
        }
    }

    void updateSelectedObjectEvent(GameObject value)
    {
        _selectedObject = value;
    }

    //clone the currently selected object and delete the editing behaviours from it
    void cloneSelectedObject()
    {
        if (_editorMode == stateManager.editorModes.cloneDeleteMode)
        {
            var clone = Instantiate(this.gameObject) as GameObject;
            clone.transform.rotation = transform.rotation;
            clone.transform.position = transform.position;

            Destroy(clone.GetComponent<universalTransform>());
            Destroy(clone.GetComponent<cloneControl>());
            Destroy(clone.GetComponent<scaleControl>());
            Destroy(clone.GetComponent<cakeslice.Outline>());
        }
    }
}
