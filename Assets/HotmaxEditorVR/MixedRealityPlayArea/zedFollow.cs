using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zedFollow : MonoBehaviour {

    public GameObject zedCamera;

    void Update ()
    {
        transform.position = zedCamera.transform.position;
        transform.rotation = zedCamera.transform.rotation;
	}
}
