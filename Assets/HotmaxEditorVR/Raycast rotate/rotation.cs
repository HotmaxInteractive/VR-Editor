using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour {

    float initialPos;
    float currentPos;
    float pieSliceIndex = 0;

    void Update()
    {

        if (Input.GetMouseButton(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.transform.parent.name == "pieHolder")
                {
                    pieSliceIndex = (float)hit.collider.transform.GetSiblingIndex();

                    transform.eulerAngles = new Vector3(pieSliceIndex * (360 / (float)hit.collider.transform.parent.childCount), 0, 0);
                }
            }
        }
    }
}
