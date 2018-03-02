using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SliderEvent : UnityEvent<float>{ }

public class slider : MonoBehaviour
{
    //--local refs
    private Transform _hand2;
    private SteamVR_TrackedController _trackedController2;

    private bool triggerDown = false;
    [SerializeField]
    public GameObject sliderCollider;
    public SliderEvent sliderUnclicked;

    private void OnEnable()
    {
        _hand2 = inputManager.hand2.gameObject.transform;
        _trackedController2 = inputManager.trackedController2;

        _trackedController2.TriggerUnclicked += triggerUnclicked;
        _trackedController2.TriggerClicked += triggerClicked;
    }

    private void OnDisable()
    {
        _trackedController2.TriggerUnclicked -= triggerUnclicked;
        _trackedController2.TriggerClicked -= triggerClicked;
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        triggerDown = true;
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        triggerDown = false;
        sliderUnclicked.Invoke(transform.localPosition.z);
    }

    void Update ()
    {
        //Thought: things like a triggerDown boolean should aybe be a global static bool to grab on to...
        if(triggerDown)
        {
            RaycastHit hit;
            Ray ray = new Ray(_hand2.transform.position, _hand2.transform.forward);
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
