using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionControl : MonoBehaviour
{
    public objectSelect objSelect;

    void Update()
    {
        if (Input.GetButton("triggerRight"))
        {
            transform.position = objSelect.endPosition;
        }
    }
}
