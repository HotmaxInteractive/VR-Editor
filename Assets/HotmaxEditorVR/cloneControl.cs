using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloneControl : MonoBehaviour
{
    public objectSelect objSelect;
    editStateController stateController;
    public float scaleSize = .5f;

    private void Start()
    {
        stateController = GetComponent<editStateController>();
    }

    private void OnEnable()
    {
        stateController.behaviorName = "Clone Object";
        objSelect.trackedController2.TriggerClicked += triggerClicked;
    }

    private void OnDisable()
    {
        objSelect.trackedController2.TriggerClicked -= triggerClicked;
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        var clone = Instantiate(this.gameObject) as GameObject;

        for (int i = 0; i < clone.GetComponent<editStateController>().components.Count; i++)
        {
            Destroy(clone.GetComponent<editStateController>().components[i]);
        }

        Destroy(clone.GetComponent<editStateController>());
        Destroy(clone.GetComponent<cakeslice.Outline>());

        clone.GetComponent<Collider>().enabled = true;
    }
}
