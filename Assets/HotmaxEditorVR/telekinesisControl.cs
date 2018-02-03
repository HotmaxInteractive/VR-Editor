using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class telekinesisControl : MonoBehaviour
{
    private float initialPadYPosition;
    private float currentPadYPos;

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
        Vector3 offset = inputManager.hand2.transform.forward * (distToController + scrollDistance);
        tweenToPosition = inputManager.hand2.transform.position + offset;

        //this gets the initial offset of the object to the controller
        //the purpose being to start the object off at the same place it was in before selecting it
        initialPadYPosition = inputManager.selectorHand.GetAxis().y;
        distToController = Vector3.Distance(transform.position, inputManager.hand2.transform.position);
        scrollDistance = 0;
        init.rotationGizmos.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        print(distToController);
        tweenDistance = Vector3.Distance(transform.position, tweenToPosition) * Time.deltaTime;
        currentPadYPos = inputManager.selectorHand.GetAxis().y;

        Vector3 offset = inputManager.hand2.transform.forward * (distToController + scrollDistance);
        tweenToPosition = inputManager.hand2.transform.position + offset;

        //--easing created by "tweenDistance" -a larger tweenDistance will make a faster tween
        transform.position = Vector3.MoveTowards(transform.position, tweenToPosition, tweenDistance * tweenSpeed);

        if (currentPadYPos > initialPadYPosition + .1f)
        {
            scrollDistance += scrollSpeed;
            initialPadYPosition = currentPadYPos;
        }

        else if (tweenToPosition.magnitude < inputManager.hand2.transform.position.magnitude * 1f)
        {
            tweenToPosition = inputManager.hand2.transform.forward * 1;
            return;
        }
        else
        {
            if (currentPadYPos < initialPadYPosition - .1f)
            {
                scrollDistance -= scrollSpeed;
                initialPadYPosition = currentPadYPos;
            }
        }
    }
}