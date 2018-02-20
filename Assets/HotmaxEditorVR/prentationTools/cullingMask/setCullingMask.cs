using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setCullingMask : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Invoke("hideLayer", 5);
	}

    private void hideLayer()
    {
        GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("hiddenFromZED"));
        GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("Gizmo Layer"));

    }
}
