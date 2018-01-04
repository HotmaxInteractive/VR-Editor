using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class spawnPageSelect : MonoBehaviour, IHittable
{
    public UnityEvent showNewPage;

    public void receiveHit(RaycastHit hit)
    {
        showNewPage.Invoke();
    }
}
