using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationControl : MonoBehaviour
{
    public objectSelect objSelect;
    GameObject parentObject;

    private void Start()
    {
        objSelect.trackedController2.TriggerClicked += triggerClicked;
        objSelect.trackedController2.TriggerUnclicked += triggerUnclicked;
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        //creating the ratcheting functionality by adding in a shell object that copies the rotation and position of the rotation control hand;
        parentObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        parentObject.transform.rotation = objSelect.hand2.transform.rotation;
        parentObject.transform.position = transform.position;
        Destroy(parentObject.GetComponent<MeshRenderer>());
        Destroy(parentObject.GetComponent<MeshFilter>());
        Destroy(parentObject.GetComponent<BoxCollider>());
        transform.parent = parentObject.transform;
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        transform.parent = null;
        Destroy(parentObject);
    }

    private void OnDestroy()
    {
        objSelect.trackedController2.TriggerClicked -= triggerClicked;
        objSelect.trackedController2.TriggerUnclicked -= triggerUnclicked;
    }

    void Update()
    {
        if (objSelect.trackedController2.triggerPressed && transform.parent != null)
        {
            transform.parent.rotation = objSelect.hand2.transform.rotation;
        }
    }
}
