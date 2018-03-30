using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliderSystem : MonoBehaviour
{
    private GameObject sliderBackground;
    private GameObject slider;
    private bool sliderBackgroundHit = false;
    public float moveAmount = 0.001f;

    private SteamVR_TrackedController _trackedController2;

    private void Start()
    {
        _trackedController2 = inputManager.trackedController2;
        _trackedController2.TriggerClicked += triggerClicked;
        _trackedController2.TriggerUnclicked += triggerUnclicked;

        sliderBackground = transform.Find("sliderBackground").gameObject;
        slider = sliderBackground.transform.Find("sliderHolder").transform.Find("slider").gameObject;
    }

    private void OnApplicationQuit()
    {
        _trackedController2.TriggerClicked -= triggerClicked;
        _trackedController2.TriggerUnclicked -= triggerUnclicked;
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        if (raycastSelect.raycastHit.collider != null)
        {
            if (raycastSelect.raycastHit.collider.gameObject == sliderBackground)
            {
                sliderBackgroundHit = true;
            }
        }
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        sliderBackgroundHit = false;
    }

    void Update()
    {
        //on trigger click, if raycast hit item is sliderBackground : true
        //...move slider...
        //on trigger up set sliderBackgroundHit : false
        if (sliderBackgroundHit)
        {
            if (raycastSelect.raycastHit.collider != null)
            {
                if (raycastSelect.raycastHit.collider.gameObject == sliderBackground)
                {
                    slider.transform.position = raycastSelect.raycastHit.point;
                    slider.transform.localPosition = new Vector3(slider.transform.localPosition.x, 0, -0.005f);
                }
            }
        }
    }

    public void moveSliderUp()
    {
        if (slider.transform.localPosition.x + moveAmount > 1)
        {
            return;
        }
        else
        {
            slider.transform.localPosition += new Vector3(moveAmount, 0, 0);
        }
    }

    public void moveSliderDown()
    {
        if (slider.transform.localPosition.x - moveAmount < 0)
        {
            return;
        }
        else
        {
            slider.transform.localPosition -= new Vector3(moveAmount, 0, 0);
        }
    }
}
