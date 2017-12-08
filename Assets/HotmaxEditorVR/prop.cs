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

    public void receiveHit(RaycastHit hit)
    {
        // selected object selection
        if (hit.collider.gameObject == _selectedObject)
        {
            _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(true);
        }
        else
        {
            _stateManagerMutatorRef.SET_SELECTED_OBJECT(hit.collider.gameObject);
            removeDecorators();
            addDecorations(hit);
        }
    }


    void addDecorations(RaycastHit raycastHit)
    {
        GameObject hitObject = null;
        hitObject = raycastHit.collider.gameObject;

        //Add object controllers and reference this class
        hitObject.AddComponent<universalTransform>();
        hitObject.AddComponent<scaleControl>();
        hitObject.AddComponent<cloneControl>();

        //Add the "selectable outline"
        hitObject.AddComponent<cakeslice.Outline>();
    }

    void removeDecorators()
    {
        //Clean up old highlighted object before adding new stuff
        cakeslice.Outline outline = (cakeslice.Outline)FindObjectOfType(typeof(cakeslice.Outline));
        if (outline)
        {
            GameObject highlightedObject = outline.transform.gameObject;
            Destroy(highlightedObject.GetComponent<universalTransform>());
            Destroy(highlightedObject.GetComponent<scaleControl>());
            Destroy(highlightedObject.GetComponent<cloneControl>());
            Destroy(outline);
        }
    }
}
