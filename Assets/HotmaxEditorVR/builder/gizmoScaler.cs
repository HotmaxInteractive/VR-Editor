using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gizmoScaler : MonoBehaviour
{
    public float sizeScaler;

	void Update ()
    {
        float distanceToHead = Vector3.Distance(transform.position, init.vrCamera.transform.position) / sizeScaler;
        transform.localScale = new Vector3(distanceToHead, distanceToHead, distanceToHead);
    }
}
