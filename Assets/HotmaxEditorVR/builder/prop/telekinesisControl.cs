using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class telekinesisControl : MonoBehaviour
{
    private float initialScrollYPosition;
    private float currentScrollYPos;

    private float distToController;
    private float scrollDistance = 0;

    [SerializeField]
    private float scrollSpeed = .2f;
    [SerializeField]
    private float tweenSpeed = 4;
    private float tweenDistance;

    private Vector3 tweenToPosition;

    private void OnEnable()
    {
        //Gets the initial offset of the object to the controller
        initialScrollYPosition = inputManager.selectorHand.GetAxis().y;
        distToController = Vector3.Distance(transform.position, inputManager.hand2.transform.position);
        scrollDistance = 0;
        init.rotationGizmos.SetActive(false);
    }

    void Update()
    {
        tweenDistance = Vector3.Distance(transform.position, tweenToPosition) * Time.deltaTime;
        currentScrollYPos = inputManager.selectorHand.GetAxis().y;

        //--Vector3.forward is 1, 0, 0 so when multiplied by the distance + scrollDistance you are left with someNumber, 0, 0
        Vector3 forwardOffsetPosition = inputManager.hand2.transform.forward * (distToController + scrollDistance);
        tweenToPosition = inputManager.hand2.transform.position + forwardOffsetPosition;
        //--easing created by "tweenDistance" -a larger tweenDistance will make a faster tween
        transform.position = Vector3.MoveTowards(transform.position, tweenToPosition, tweenDistance * tweenSpeed);

        if (currentScrollYPos > initialScrollYPosition + .1f)
        {
            scrollDistance += scrollSpeed;
            initialScrollYPosition = currentScrollYPos;
        }

        if (currentScrollYPos < initialScrollYPosition - .1f)
        {
            scrollDistance -= scrollSpeed;
            initialScrollYPosition = currentScrollYPos;
        }
    }
}