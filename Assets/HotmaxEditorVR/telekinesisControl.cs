using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class telekinesisControl : MonoBehaviour
{

    private float initialPadYPosition;
    private float currentPadYPos;

    private float distToController;
    [SerializeField]
    private float scrollSpeed = .2f;
    private float scrollDistance = 0;


    private void OnEnable()
    {
        //this gets the initial offset of the object to the controller
        //the purpose being to start the object off at the same place it was in before selecting it
        initialPadYPosition = inputManager.selectorHand.GetAxis().y;
        distToController = Vector3.Distance(transform.position, inputManager.hand2.transform.position);
        scrollDistance = 0;
    }

    // Update is called once per frame
    void Update ()
    {
        init.rotationGizmos.SetActive(false);

        currentPadYPos = inputManager.selectorHand.GetAxis().y;

        Vector3 offset = inputManager.hand2.transform.forward * (distToController + scrollDistance);
        transform.position = inputManager.hand2.transform.position + offset;

        if (currentPadYPos > initialPadYPosition)
        {
            scrollDistance += scrollSpeed;
            initialPadYPosition = currentPadYPos;
        }
        if (currentPadYPos < initialPadYPosition)
        {
            scrollDistance -= scrollSpeed;
            initialPadYPosition = currentPadYPos;
        }
    }
}
