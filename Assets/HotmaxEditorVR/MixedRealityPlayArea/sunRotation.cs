using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunRotation : MonoBehaviour
{
    public Light directionalLight;

    private bool sunIsBeingHeld = false;

    public void sunPickedUp()
    {
        sunIsBeingHeld = true;
    }

    public void sunSetDown()
    {
        sunIsBeingHeld = false;
    }

    private void Update()
    {
        if(!sunIsBeingHeld)
        {
            return;
        }
        else
        {
            directionalLight.transform.rotation = transform.rotation;
        }
    }
}
