using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setCullingMask : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Invoke("hideLayer", 3);
	}

    private void hideLayer()
    {
        GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("hiddenFromZED"));
    }
}
