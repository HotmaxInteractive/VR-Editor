using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class materialPageSelect : MonoBehaviour, IHittable
{
    public UnityEvent showNewMaterialPage;

    public void receiveHit(RaycastHit hit)
    {
        showNewMaterialPage.Invoke();
    }
}