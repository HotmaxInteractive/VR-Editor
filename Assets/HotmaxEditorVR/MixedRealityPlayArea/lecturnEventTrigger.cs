using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lecturnEventTrigger : MonoBehaviour
{
    private bool moveObjects = false;
    private bool objectsAreInDownPosition = false;

    public Transform screen;
    private Vector3 upScreenPosition;
    private Vector3 downScreenPosition;
    public Transform screenMoveToPosition;

    public Transform displayCases;
    private Vector3 upDisplayCasesPosition;
    private Vector3 downDisplayCasesPosition;
    public Transform displayCasesMoveToPosition;

    public float steps = 1f;

    public GameObject mainLights;

    private void Start()
    {
        upScreenPosition = screen.transform.position;
        downScreenPosition = screenMoveToPosition.position;

        upDisplayCasesPosition = displayCases.transform.position;
        downDisplayCasesPosition = displayCasesMoveToPosition.position;
    }

    void Update()
    {
        if(moveObjects)
        {
            float tweenDistance = Vector3.Distance(transform.position, screenMoveToPosition.position) * Time.deltaTime;
            screen.position = Vector3.MoveTowards(screen.position, screenMoveToPosition.position, steps * tweenDistance);
            displayCases.position = Vector3.MoveTowards(displayCases.position, displayCasesMoveToPosition.position, steps * tweenDistance);

            if (Vector3.Distance(screen.position, screenMoveToPosition.position) < .01f)
            {
                moveObjects = false;
                objectsAreInDownPosition = !objectsAreInDownPosition;
                if (objectsAreInDownPosition)
                {
                    screenMoveToPosition.position = upScreenPosition;
                    displayCasesMoveToPosition.position = upDisplayCasesPosition;
                    mainLights.SetActive(false);
                }
                else
                {
                    screenMoveToPosition.position = downScreenPosition;
                    displayCasesMoveToPosition.position = downDisplayCasesPosition;
                    mainLights.SetActive(true);
                }
            }
        }
    }

    public void videoScreenDown()
    {
        moveObjects = true;
    }
}
