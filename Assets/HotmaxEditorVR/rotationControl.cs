using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationControl : MonoBehaviour
{
    public objectSelect objSelect;
    Vector3 handInitRotation;
    Vector3 objInitRotation;
    editStateController stateController;

    private void Start()
    {
        stateController = GetComponent<editStateController>();
    }

    private void OnEnable()
    {
        stateController.behaviorName = "Rotation";
    }

    void Update()
    {
        //When having the trigger down
        //Add to the existing rotation
        //Add the hand's "moved amount" to the Cube
        //rotation of Cube = initial rotation of Cube + hand's rotational difference
        getObjectInitialRotation();

        if (Input.GetKey(KeyCode.JoystickButton15))
        {
            float moveDistX = objSelect.hand2.transform.eulerAngles.x - handInitRotation.x;
            float moveDistY = objSelect.hand2.transform.eulerAngles.y - handInitRotation.y;
            float moveDistZ = objSelect.hand2.transform.eulerAngles.z - handInitRotation.z;

            transform.eulerAngles = new Vector3(objInitRotation.x + moveDistX, objInitRotation.y + moveDistY, objInitRotation.z + moveDistZ);
        }
    }

    void getObjectInitialRotation()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton15))
        {
            handInitRotation = objSelect.hand2.transform.eulerAngles;
            objInitRotation = transform.eulerAngles;
        }
    }
}
