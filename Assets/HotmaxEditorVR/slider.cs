using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class slider : MonoBehaviour {

    private bool triggerDown = false;
    public GameObject sliderCollider;

    public UnityEvent sliderUnclicked;

    private void OnEnable()
    {
        inputManager.trackedController2.TriggerUnclicked += triggerUnclicked;
        inputManager.trackedController2.TriggerClicked += triggerClicked;
    }

    private void OnDisable()
    {
        inputManager.trackedController2.TriggerUnclicked -= triggerUnclicked;
        inputManager.trackedController2.TriggerClicked -= triggerClicked;
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        triggerDown = true;
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        triggerDown = false;
        sliderUnclicked.Invoke();
    }

    void Update ()
    {
        //Thought: things like a triggerDown boolean should aybe be a global static bool to grab on to...
        if(triggerDown)
        {
            RaycastHit hit;
            Ray ray = new Ray(inputManager.hand2.gameObject.transform.position, inputManager.hand2.gameObject.transform.forward);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.transform == sliderCollider.transform)
                {
                    transform.position = hit.point;
                    transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
                }
            }
        }
    }
}
