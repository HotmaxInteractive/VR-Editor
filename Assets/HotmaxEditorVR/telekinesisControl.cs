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

    private GameObject tweenToPosition;

    private void OnEnable()
    {
        tweenToPosition = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tweenToPosition.GetComponent<MeshRenderer>().enabled = false;
        tweenToPosition.GetComponent<BoxCollider>().enabled = false;

        //this gets the initial offset of the object to the controller
        //the purpose being to start the object off at the same place it was in before selecting it
        initialPadYPosition = inputManager.selectorHand.GetAxis().y;
        distToController = Vector3.Distance(transform.position, inputManager.hand2.transform.position);
        scrollDistance = 0;
        init.rotationGizmos.SetActive(false);
    }

    private void OnDisable()
    {
        Destroy(tweenToPosition);
    }

    // Update is called once per frame
    void Update()
    {
        tweenDistance = Vector3.Distance(transform.position, tweenToPosition.transform.position) * Time.deltaTime;
        currentPadYPos = inputManager.selectorHand.GetAxis().y;

        Vector3 offset = inputManager.hand2.transform.forward * (distToController + scrollDistance);
        tweenToPosition.transform.position = inputManager.hand2.transform.position + offset;
        //--easing created by "tweenDistance" -a larger tweenDistance will make a faster tween
        transform.position = Vector3.MoveTowards(transform.position, tweenToPosition.transform.position, tweenDistance * tweenSpeed);

        if (currentPadYPos > initialPadYPosition + .1f)
        {
            scrollDistance += scrollSpeed;
            initialPadYPosition = currentPadYPos;
        }

        if (tweenToPosition.transform.position.magnitude < inputManager.hand2.transform.position.magnitude + 0.3f)
        {
            tweenToPosition.transform.position = inputManager.hand2.transform.forward / 2;
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