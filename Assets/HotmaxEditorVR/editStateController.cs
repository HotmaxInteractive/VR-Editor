using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editStateController : MonoBehaviour {

    rotationControl rotControl;
    positionControl posControl;
    scaleControl scalControl;

	void Start () {
        rotControl = GetComponent<rotationControl>();
        posControl = GetComponent<positionControl>();
        scalControl = GetComponent<scaleControl>();
	}
	
	void Update () {
        if (Input.GetButtonDown("menuRight"))
        {
            print("menuRight ??");
        }
	}
}
