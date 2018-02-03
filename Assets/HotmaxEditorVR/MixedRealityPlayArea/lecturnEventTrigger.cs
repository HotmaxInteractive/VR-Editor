using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lecturnEventTrigger : MonoBehaviour
{
    private bool moveScreen = false;
    private bool screenIsInDownPosition = false;
    public Transform screen;
    private Vector3 upScreenPosition;
    private Vector3 downScreenPosition;

    public Transform moveToPosition;
    public float steps = 1f;

    private void Start()
    {
        upScreenPosition = screen.transform.position;
        downScreenPosition = moveToPosition.position;
    }

    void Update()
    {
        if(moveScreen)
        {
            float tweenDistance = Vector3.Distance(transform.position, moveToPosition.position) * Time.deltaTime;
            screen.position = Vector3.MoveTowards(screen.position, moveToPosition.position, steps * tweenDistance);

            if (Vector3.Distance(screen.position, moveToPosition.position) < .01f)
            {
                moveScreen = false;
                screenIsInDownPosition = !screenIsInDownPosition;
                if (screenIsInDownPosition)
                {
                    moveToPosition.position = upScreenPosition;
                }
                else
                {
                    moveToPosition.position = downScreenPosition;
                }
            }
        }
    }

    public void videoScreenDown()
    {
        moveScreen = true;
    }
}
