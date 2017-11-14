using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionControl : MonoBehaviour
{
    public objectSelect objSelect;
    editStateController stateController;

    Vector3 handInitPosition;
    Vector3 objInitPosition;

    private void Start()
    {
        stateController = GetComponent<editStateController>();
    }

    private void OnEnable()
    {
        stateController.behaviorName = "Position";

        objSelect.trackedController2.TriggerClicked += triggerClicked;
    }

    private void OnDisable()
    {
        objSelect.trackedController2.TriggerClicked -= triggerClicked;
    }

    void Update()
    {
        if (objSelect.trackedController2.triggerPressed)
        {
            float moveDistX = objSelect.hand2.transform.position.x - handInitPosition.x;
            float moveDistY = objSelect.hand2.transform.position.y - handInitPosition.y;
            float moveDistZ = objSelect.hand2.transform.position.z - handInitPosition.z;

            transform.position = new Vector3(objInitPosition.x + moveDistX, objInitPosition.y + moveDistY, objInitPosition.z + moveDistZ);
        }
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        handInitPosition = objSelect.hand2.transform.position;
        objInitPosition = transform.position;
    }
}
