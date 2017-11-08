using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateControl : MonoBehaviour {

    public objectSelect objSelect;

    void Update()
    {
        transform.rotation = objSelect.hand1.transform.rotation;
    }
}
