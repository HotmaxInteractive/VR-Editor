using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prop : MonoBehaviour, IHittable
{

    //--local refs
    private stateManager _stateManagerMutatorRef;
    private GameObject _selectedObject;
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private stateManager.editorModes _editorMode = stateManager.editorMode;

    //--put the monobehaviours into a list
    private List<MonoBehaviour> editBehaviours = new List<MonoBehaviour>();


    private void Awake()
    {
        _stateManagerMutatorRef = GameObject.FindObjectOfType(typeof(stateManager)) as stateManager;
        stateManager.selectedObjectEvent += updateSelectedObject;
        stateManager.editorModeEvent += updateEditorMode;
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;


    }

    protected virtual void OnApplicationQuit()
    {
        stateManager.selectedObjectEvent -= updateSelectedObject;
        stateManager.editorModeEvent -= updateEditorMode;
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
    }

    void updateSelectedObject(GameObject value)
    {
        _selectedObject = value;
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;

        if (this.gameObject == _selectedObject)
        {
            checkEnabledBehaviors();
        }
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
            removeDecorators();
            addDecorations();
        }
    }

    void addDecorations()
    {
       
        //Add object controllers and reference this class
        this.gameObject.AddComponent<rotationControl>();
        this.gameObject.AddComponent<telekinesisControl>();
        this.gameObject.AddComponent<cloneControl>();
        this.gameObject.AddComponent<scaleControl>();

        editBehaviours.Add(GetComponent<rotationControl>());
        editBehaviours.Add(GetComponent<telekinesisControl>());
        editBehaviours.Add(GetComponent<cloneControl>());
        editBehaviours.Add(GetComponent<scaleControl>());

        //disable all of them and check editor mode to set active
        checkEnabledBehaviors();


        //Add the "selectable outline"
        this.gameObject.AddComponent<cakeslice.Outline>();
    }

    void removeDecorators()
    {
        //Clean up old highlighted object before adding new stuff
        cakeslice.Outline outline = (cakeslice.Outline)FindObjectOfType(typeof(cakeslice.Outline));
        if (outline)
        {
            GameObject highlightedObject = outline.transform.gameObject;
            Destroy(highlightedObject.GetComponent<rotationControl>());
            Destroy(highlightedObject.GetComponent<telekinesisControl>());
            Destroy(highlightedObject.GetComponent<scaleControl>());
            Destroy(highlightedObject.GetComponent<cloneControl>());

            editBehaviours.Remove(GetComponent<rotationControl>());
            editBehaviours.Remove(GetComponent<telekinesisControl>());
            editBehaviours.Remove(GetComponent<cloneControl>());
            editBehaviours.Remove(GetComponent<scaleControl>());

            Destroy(outline);
        }
    }

    private void Update()
    {
        
    }


    void updateEditorMode(stateManager.editorModes value)
    {
        _editorMode = value;

        if (this.gameObject == _selectedObject)
        {
            checkEnabledBehaviors();
        }
    }

    private void checkEnabledBehaviors()
    {
        switch (_editorMode)
        {
            case stateManager.editorModes.universalTransformMode:
                for (int i = 0; i < editBehaviours.Count; i++)
                {
                    editBehaviours[i].enabled = false;
                }

                if (_selectedObjectIsActive)
                {
                    //turns on telekinesis
                    editBehaviours[1].enabled = true;
                }
                else
                {
                    //turns on rotation
                    editBehaviours[0].enabled = true;
                }
                break;

            case stateManager.editorModes.cloneDeleteMode:
                for (int i = 0; i < editBehaviours.Count; i++)
                {
                    editBehaviours[i].enabled = false;
                }
                editBehaviours[2].enabled = true;
                break;

            case stateManager.editorModes.openMenuMode:
                for (int i = 0; i < editBehaviours.Count; i++)
                {
                    editBehaviours[i].enabled = false;
                }
                editBehaviours[3].enabled = true;
                break;
        }
    }
}

