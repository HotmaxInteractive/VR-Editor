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

    //void updateSelectedObjectIsActive(bool value)
    //{
    //    _selectedObjectIsActive = value;

    //    if (this.gameObject == _selectedObject)
    //    {
    //        checkEnabledBehaviors();
    //    }

    //    if(_selectedObjectIsActive)
    //    {
    //        init.deletedProps.SetActive(false);
    //    }
    //}

    //void updateEditorMode(stateManager.editorModes value)
    //{
    //    _editorMode = value;

    //    if (this.gameObject == _selectedObject)
    //    {
    //        checkEnabledBehaviors();
    //    }

    //    if (_editorMode == stateManager.editorModes.cloneDeleteMode)
    //    {
    //        init.deletePanel.SetActive(true);
    //    }
    //    else
    //    {
    //        init.deletePanel.SetActive(false);
    //    }
    //}

    public void receiveHit(RaycastHit hit)
    {
        // selected object selection
        if (this.gameObject == _selectedObject)
        {
            print("object is active");
            _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(true);
        }

       
        // if new prop is selected
        else
        {
            print("new object is selected");

            //removeDecorators();
            _stateManagerMutatorRef.SET_SELECTED_OBJECT(hit.collider.gameObject);
            //addDecorations();

            this.gameObject.AddComponent<activeProp>();
        }
    }

    //void addDecorations()
    //{
    //    //Add object controllers and reference this class
    //    this.gameObject.AddComponent<rotationControl>();
    //    this.gameObject.AddComponent<telekinesisControl>();
    //    this.gameObject.AddComponent<scaleControl>();
    //    //Add the "selectable outline"
    //    this.gameObject.AddComponent<cakeslice.Outline>();

    //    editBehaviours.Add(GetComponent<rotationControl>());
    //    editBehaviours.Add(GetComponent<telekinesisControl>());
    //    editBehaviours.Add(GetComponent<scaleControl>());

    //    //disable all of them and check editor mode to set active
    //    checkEnabledBehaviors();
    //}

    //void removeDecorators()
    //{
    //    //Clean up old highlighted object before adding new stuff
    //    //Optimize this later FindObjectOfType is slow, probably by using _selectedObject
    //    cakeslice.Outline outline = (cakeslice.Outline)FindObjectOfType(typeof(cakeslice.Outline));
    //    if (outline)
    //    {
    //        GameObject highlightedObject = outline.transform.gameObject;
    //        Destroy(highlightedObject.GetComponent<rotationControl>());
    //        Destroy(highlightedObject.GetComponent<telekinesisControl>());
    //        Destroy(highlightedObject.GetComponent<scaleControl>());

    //        editBehaviours.Remove(GetComponent<rotationControl>());
    //        editBehaviours.Remove(GetComponent<telekinesisControl>());
    //        editBehaviours.Remove(GetComponent<scaleControl>());

    //        Destroy(outline);
    //    }
    //}

    //maybe this is the decoration manager...
    //private void checkEnabledBehaviors()
    //{
    //    switch (_editorMode)
    //    {
    //        case stateManager.editorModes.universalTransformMode:
    //            for (int i = 0; i < editBehaviours.Count; i++)
    //            {
    //                editBehaviours[i].enabled = false;
    //            }

    //            if (_selectedObjectIsActive)
    //            {
    //                //turns on telekinesis
    //                editBehaviours[1].enabled = true;
    //            }
    //            else
    //            {
    //                //turns on rotation
    //                editBehaviours[0].enabled = true;
    //            }
    //            break;

    //        case stateManager.editorModes.cloneDeleteMode:
    //            for (int i = 0; i < editBehaviours.Count; i++)
    //            {
    //                editBehaviours[i].enabled = false;
    //            }

    //            deleteObjectHandler();

    //            if (_selectedObjectIsActive)
    //            {
    //                //turn on the cloning behaviour
    //                cloneSelectedObject();
    //                //turn on telekinesis behaviour
    //                editBehaviours[1].enabled = true;
    //            }
    //            break;

    //        case stateManager.editorModes.openMenuMode:
    //            for (int i = 0; i < editBehaviours.Count; i++)
    //            {
    //                editBehaviours[i].enabled = false;
    //            }

    //            editBehaviours[2].enabled = true;
    //            break;
    //    }
    //}

    //void cloneSelectedObject()
    //{
    //    var clone = Instantiate(this.gameObject) as GameObject;
    //    clone.transform.rotation = transform.rotation;
    //    clone.transform.position = transform.position;
    //    clone.transform.parent = init.props.transform;

    //    Destroy(clone.GetComponent<rotationControl>());
    //    Destroy(clone.GetComponent<telekinesisControl>());
    //    Destroy(clone.GetComponent<scaleControl>());
    //    Destroy(clone.GetComponent<cakeslice.Outline>());

    //    clone.name = this.gameObject.name;
    //}

    //void deleteObjectHandler()
    //{
    //    if(_selectedObject.activeInHierarchy)
    //    {
    //        init.deletePanel.SetActive(true);
    //        Vector3 selectedObjectTransform = new Vector3(_selectedObject.transform.position.x, _selectedObject.transform.position.y + 1, _selectedObject.transform.position.z);
    //        init.deletePanel.transform.position = selectedObjectTransform;
    //    }
    //}
}

