using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gizmoScaler : MonoBehaviour
{
    //--local refs
    private GameObject _vrCamera;

    public float sizeScaler;

    private void Start()
    {
        _vrCamera = init.vrCamera;
    }

    void Update ()
    {
        float distanceToHead = Vector3.Distance(transform.position, _vrCamera.transform.position);
        float scaleRatio = distanceToHead / sizeScaler;
        transform.localScale = new Vector3(scaleRatio, scaleRatio, scaleRatio);
    }
}
