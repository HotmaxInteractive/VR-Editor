using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class massScale : MonoBehaviour, IHittable
{
    public UnityEvent moveSlider;

    public void receiveHit(RaycastHit hit)
    {
        moveSlider.Invoke();       
    }
}
