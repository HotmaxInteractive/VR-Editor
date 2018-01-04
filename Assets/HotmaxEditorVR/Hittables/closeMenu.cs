using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class closeMenu : MonoBehaviour, IHittable
{
    public UnityEvent removeMenu;

    public void receiveHit(RaycastHit hit)
    {
        removeMenu.Invoke();
    }
}
