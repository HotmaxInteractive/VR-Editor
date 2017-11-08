using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayPointFollow : MonoBehaviour {

    public objectSelect objSelect;

	void Update () {
        if (Input.GetButton("Fire2"))
        {
            transform.position = objSelect.endPosition;
        }
    }
}
