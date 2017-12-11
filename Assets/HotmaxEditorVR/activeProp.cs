using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeProp : MonoBehaviour
{

    //--local refs
    private GameObject _selectedObject = stateManager.selectedObject;
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private stateManager.editorModes _editorMode = stateManager.editorMode;

    //--put the monobehaviours into a list
    private List<MonoBehaviour> decorators = new List<MonoBehaviour>();

    private void OnEnable()
    {
        stateManager.selectedObjectEvent += updateSelectedObject;
        stateManager.editorModeEvent += updateEditorMode;
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
        addDecorations();
    }

    private void OnDisable()
    {
        stateManager.selectedObjectEvent -= updateSelectedObject;
        stateManager.editorModeEvent -= updateEditorMode;
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
        removeDecorators();
    }

    //this only fires when a new object is selected
    void updateSelectedObject(GameObject value)
    {
        _selectedObject = value;
        Destroy(this);
    }

    private void Awake()
    {
        //--This is a check for if the instantiated object is a clone
        if (this.gameObject != _selectedObject)
        {
            Destroy(this);
        }
    }


    // -- ENTRY POINT : this object is selected
    //
    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
        activePropStateMachine();
    }

    // -- ENTRY POINT : editor mode has changed
    //
    void updateEditorMode(stateManager.editorModes value)
    {
        _editorMode = value;
        activePropStateMachine();
    }


    private void activePropStateMachine()
    {
        for (int i = 0; i < decorators.Count; i++)
        {
            decorators[i].enabled = false;
        }
        init.deletePanel.SetActive(false);

        switch (_editorMode)
        {
            case stateManager.editorModes.universalTransformMode:
                if (_selectedObjectIsActive)
                {
                    //turns on telekinesis
                    decorators[1].enabled = true;
                }
                else
                {
                    //turns on rotation
                    decorators[0].enabled = true;
                }
                break;
            case stateManager.editorModes.cloneDeleteMode:
                deleteObjectHandler();
                if (_selectedObjectIsActive)
                {
                    //turn on the cloning behaviour
                    cloneSelectedObject();
                    //turn on telekinesis behaviour
                    decorators[1].enabled = true;
                }
                else
                {
                    init.deletePanel.SetActive(true);
                }
                break;
            case stateManager.editorModes.openMenuMode:
                decorators[2].enabled = true;
                break;
        }
    }

    private void checkEnabledDecorator()
    {
        for (int i = 0; i < decorators.Count; i++)
        {
            decorators[i].enabled = false;
        }

        switch (_editorMode)
        {
            case stateManager.editorModes.universalTransformMode:
                decorators[0].enabled = true; 
                break;
            case stateManager.editorModes.cloneDeleteMode:
                //nothing
                break;
            case stateManager.editorModes.openMenuMode:
                decorators[2].enabled = true;
                break;
        }
    }



    void addDecorations()
    {
        //Add object controllers and reference this class
        this.gameObject.AddComponent<rotationControl>();
        this.gameObject.AddComponent<telekinesisControl>();
        this.gameObject.AddComponent<scaleControl>();
        //Add the "selectable outline"
        this.gameObject.AddComponent<cakeslice.Outline>();

        //adding the 
        decorators.Add(GetComponent<rotationControl>());
        decorators.Add(GetComponent<telekinesisControl>());
        decorators.Add(GetComponent<scaleControl>());

        //disable all of them and check editor mode to set active
        checkEnabledDecorator();
    }

    void removeDecorators()
    {
        Destroy(GetComponent<rotationControl>());
        Destroy(GetComponent<telekinesisControl>());
        Destroy(GetComponent<scaleControl>());
        Destroy(GetComponent<cakeslice.Outline>());
    }

    void cloneSelectedObject()
    {
        var clone = Instantiate(this.gameObject) as GameObject;
        clone.transform.rotation = transform.rotation;
        clone.transform.position = transform.position;
        clone.transform.parent = init.props.transform;

        clone.name = this.gameObject.name;
    }

    void deleteObjectHandler()
    {
        init.deletePanel.SetActive(true);
        Vector3 selectedObjectTransform = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        init.deletePanel.transform.position = selectedObjectTransform;        
    }
}
