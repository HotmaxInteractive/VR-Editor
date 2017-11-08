using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateControl : MonoBehaviour
{
    public objectSelect objSelect;
    Vector3 handInitRotation;
    Vector3 objInitRotation;

    void Update()
    {
        //When having the trigger down
        //Add to the existing rotation
        //Add the hand's "moved amount" to the Cube
        //rotation of Cube = initial rotation of Cube + hand's rotational difference
        getObjectInitialRotation();

        if (Input.GetButton("Fire1"))
        {

            float moveDistX = objSelect.hand1.transform.eulerAngles.x - handInitRotation.x;
            float moveDistY = objSelect.hand1.transform.eulerAngles.y - handInitRotation.y;
            float moveDistZ = objSelect.hand1.transform.eulerAngles.z - handInitRotation.z;

            transform.eulerAngles = new Vector3(objInitRotation.x + moveDistX, objInitRotation.y + moveDistY, objInitRotation.z + moveDistZ);
        }
    }

    void getObjectInitialRotation()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            handInitRotation = objSelect.hand1.transform.eulerAngles;
            objInitRotation = transform.eulerAngles;
        }
    }
}
