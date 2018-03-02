using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeProp : MonoBehaviour
{

    //--local refs
    private GameObject _selectedObject = stateManager.selectedObject;
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private stateManager.editorModes _editorMode = stateManager.editorMode;
    private GameObject _deletePanel;
    private GameObject _props;

    //--put the monobehaviours into a list
    Dictionary<string, MonoBehaviour> decorators = new Dictionary<string, MonoBehaviour>();

    private void OnEnable()
    {
        stateManager.selectedObjectEvent += updateSelectedObject;
        stateManager.editorModeEvent += updateEditorMode;
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;

        _deletePanel = init.deletePanel;
        _props = init.props;

        addDecorations();
    }

    private void OnDisable()
    {
        stateManager.selectedObjectEvent -= updateSelectedObject;
        stateManager.editorModeEvent -= updateEditorMode;
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
        removeDecorators();
    }

    private void Awake()
    {
        //--This is a check for if the instantiated object is a clone
        if (gameObject != _selectedObject)
        {
            Destroy(this);
        }
    }

    //destroy this component (and all decorators) when selected object changes
    void updateSelectedObject(GameObject value)
    {
        _selectedObject = value;
        Destroy(this);
    }

    // -- RUN STATE MACHINE : editor mode has changed
    void updateEditorMode(stateManager.editorModes value)
    {
        _editorMode = value;
        runPropStateMachine();
    }

    // -- RUN STATE MACHINE : is active or made in-active
    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
        runPropStateMachine();
    }

    private void runPropStateMachine()
    {
        foreach (MonoBehaviour decorator in decorators.Values)
        {
            decorator.enabled = false;
        }
        _deletePanel.SetActive(false);

        switch (_editorMode)
        {
            case stateManager.editorModes.universalTransformMode:
                if (_selectedObjectIsActive)
                {
                    decorators["positionControl"].enabled = true;
                }
                else
                {
                    decorators["rotationControl"].enabled = true;
                    decorators["scaleControl"].enabled = true;
                }
                break;
            case stateManager.editorModes.cloneDeleteMode:
                deleteObjectHandler();
                if (_selectedObjectIsActive)
                {
                    //turn on the cloning behaviour
                    cloneSelectedObject();
                    decorators["positionControl"].enabled = true;
                }
                else
                {
                    _deletePanel.SetActive(true);
                }
                break;
            case stateManager.editorModes.openMenuMode:
                if (_selectedObjectIsActive)
                {
                    decorators["positionControl"].enabled = true;
                }
                break;
        }
    }

    private void checkEnabledDecorator()
    {
        foreach (MonoBehaviour decorator in decorators.Values)
        {
            decorator.enabled = false;
        }

        switch (_editorMode)
        {
            case stateManager.editorModes.universalTransformMode:
                decorators["rotationControl"].enabled = true;
                break;
            case stateManager.editorModes.cloneDeleteMode:
                //nothing
                break;
            case stateManager.editorModes.openMenuMode:
                decorators["scaleControl"].enabled = true;
                break;
        }
    }

    void addDecorations()
    {
        //Add object controllers and reference this class
        gameObject.AddComponent<rotationControl>();
        gameObject.AddComponent<positionControl>();
        gameObject.AddComponent<scaleControl>();

        //Add the "selectable outline"
        gameObject.AddComponent<cakeslice.Outline>();

        //adding the 
        decorators.Add("rotationControl", GetComponent<rotationControl>());
        decorators.Add("positionControl", GetComponent<positionControl>());
        decorators.Add("scaleControl", GetComponent<scaleControl>());

        //disable all of them and check editor mode to set active
        checkEnabledDecorator();
    }

    void removeDecorators()
    {
        Destroy(GetComponent<rotationControl>());
        Destroy(GetComponent<positionControl>());
        Destroy(GetComponent<scaleControl>());
        Destroy(GetComponent<cakeslice.Outline>());
    }

    void cloneSelectedObject()
    {
        var clone = Instantiate(gameObject) as GameObject;
        clone.transform.rotation = transform.rotation;
        clone.transform.position = transform.position;
        clone.transform.parent = _props.transform;

        clone.name = gameObject.name;
    }

    void deleteObjectHandler()
    {
        _deletePanel.SetActive(true);
        Vector3 selectedObjectTransform = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        _deletePanel.transform.position = selectedObjectTransform;        
    }
}
