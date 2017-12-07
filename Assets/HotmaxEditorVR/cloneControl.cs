using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloneControl : MonoBehaviour
{
    public objectSelect objSelect;
    public float scaleSize = .5f;
    bool cloneMode = false;
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private stateManager.editorModes _editorMode = stateManager.editorMode;
    GameObject _selectedObject;


    private void Start()
    {
        objSelect.trackedController2.TriggerClicked += triggerClicked;
        stateManager.editorModeEvent += updateEditorMode;
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
        stateManager.selectedObjectEvent += updateSelectedObjectEvent;
    }

    protected virtual void OnApplicationQuit()
    {
        objSelect.trackedController2.TriggerClicked -= triggerClicked;
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
    }

    //MAYBE: if editor mode clone delete, update hit object. take the new value and clone it
    void updateSelectedObjectEvent(GameObject value)
    {
        _selectedObject = value;
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        if (_editorMode == stateManager.editorModes.cloneDeleteMode)
        {

            var clone = Instantiate(this.gameObject) as GameObject;
            clone.transform.rotation = transform.rotation;
            clone.transform.position = transform.position;

            //TODO: now that we have the clone, add decorators to it

            Destroy(clone.GetComponent<universalTransform>());
            Destroy(clone.GetComponent<cloneControl>());
            Destroy(clone.GetComponent<scaleControl>());
            Destroy(clone.GetComponent<cakeslice.Outline>());
        }
    }
}
