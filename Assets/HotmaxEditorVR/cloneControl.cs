using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloneControl : MonoBehaviour
{
    private GameObject _selectedObject;

    void updateSelectedObject(GameObject value)
    {
        _selectedObject = value;
    }

    private void OnEnable()
    {
        stateManager.selectedObjectEvent += updateSelectedObject;

        //_selectedObjectIsActive is already firing, and the selected object (this) is not yet passed in
        //TODO: this is probably a bad way to do this, if object has string "Clone" in name it breaks
        if (!this.gameObject.name.Contains("Clone"))
        {
            cloneSelectedObject();
        }
    }

    private void OnDisable()
    {
        stateManager.selectedObjectEvent -= updateSelectedObject;
    }

    //clone the currently selected object and delete the editing behaviours from it
    void cloneSelectedObject()
    {
        var clone = Instantiate(this.gameObject) as GameObject;
        clone.transform.rotation = transform.rotation;
        clone.transform.position = transform.position;
        clone.transform.parent = init.props.transform;

        Destroy(clone.GetComponent<rotationControl>());
        Destroy(clone.GetComponent<telekinesisControl>());
        Destroy(clone.GetComponent<cloneControl>());
        Destroy(clone.GetComponent<scaleControl>());
        Destroy(clone.GetComponent<cakeslice.Outline>());

        clone.name = this.gameObject.name;
        this.enabled = false;
    }
}
