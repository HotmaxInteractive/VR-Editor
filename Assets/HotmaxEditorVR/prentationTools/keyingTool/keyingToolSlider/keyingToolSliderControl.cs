using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyingToolSliderControl : MonoBehaviour
{
    public GameObject sliderBackground;
    public GameObject slider;

    void Update()
    {
        if (inputManager.trackedController2.triggerPressed)
        {
            RaycastHit hit;
            Ray ray = new Ray(inputManager.hand2.gameObject.transform.position, inputManager.hand2.gameObject.transform.forward);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.gameObject == sliderBackground)
                {
                    slider.transform.position = hit.point;
                    slider.transform.localPosition = new Vector3(slider.transform.localPosition.x, 0, -0.005f);
                }
            }
        }
    }

    public void moveSliderUp()
    {
        float moveAmount = 0.001f;
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
        float moveAmount = 0.001f;
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

