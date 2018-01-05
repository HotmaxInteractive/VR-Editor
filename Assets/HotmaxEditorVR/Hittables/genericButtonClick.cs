using UnityEngine.Events;
using UnityEngine;

public class genericButtonClick : MonoBehaviour, IHittable
{
    public UnityEvent buttonClicked;

    public void receiveHit(RaycastHit hit)
    {
        buttonClicked.Invoke();
    }
}
