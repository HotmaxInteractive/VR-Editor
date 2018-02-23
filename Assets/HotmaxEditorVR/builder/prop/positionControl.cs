using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionControl : MonoBehaviour
{
    private float initialScrollYPosition;
    private float currentScrollYPos;

    private float initialDistanceToController;
    private float scrollDistance = 0;

    [SerializeField]
    private float scrollSpeed = .2f;
    [SerializeField]
    private float tweenSpeed = 4;
    private float tweenDistance;

    private Vector3 tweenToPosition;

    private GameObject _objectCollidedWithHand;
    private Transform initialParent;

    private void OnEnable()
    {
        stateManager.objectCollidedWithHandEvent += updateObjectCollidedWithHand;

        initialScrollYPosition = inputManager.selectorHand.GetAxis().y;
        initialDistanceToController = Vector3.Distance(transform.position, inputManager.hand2.transform.position);
        scrollDistance = 0;
        init.rotationGizmos.SetActive(false);

        //--the updateObjectCollidedWithHand event wont fire on this class during its lifetime
        _objectCollidedWithHand = stateManager.objectCollidedWithHand;
    }

    private void OnDisable()
    {
        stateManager.objectCollidedWithHandEvent -= updateObjectCollidedWithHand;
        transform.parent = init.props.transform;
    }

    void updateObjectCollidedWithHand(GameObject value)
    {
        _objectCollidedWithHand = value;
    }

    void Update()
    {
        currentScrollYPos = inputManager.selectorHand.GetAxis().y;

        if (_objectCollidedWithHand == gameObject)
        {
            if(inputManager.trackedController2.padTouched)
            {
                reparentPropInProps();
                propOffsetController(true);
            }
            else
            {
                parentPropInHand();
            }
            return;
        }
        else
        {
            propOffsetController(false);
        }
    }

    void parentPropInHand()
    {
        if (transform.parent != inputManager.hand2.transform)
        {
            transform.parent = inputManager.hand2.transform;
        }
    }

    void reparentPropInProps()
    {
        transform.parent = init.props.transform;
    }

    void propOffsetController(bool currentlyInHand)
    {   
        //previous frame
        float propDistanceToController = initialDistanceToController + scrollDistance;
        if (currentScrollYPos > initialScrollYPosition + .1f)
        {
            scrollDistance += scrollSpeed;
            initialScrollYPosition = currentScrollYPos;
        }

        if(propDistanceToController >= 0)
        {
            if (currentScrollYPos < initialScrollYPosition - .1f)
            {
                scrollDistance -= scrollSpeed;
                initialScrollYPosition = currentScrollYPos;
            }
        }

        //after updated scrollDistance
        propDistanceToController = initialDistanceToController + scrollDistance;
        //--Creates a local controller forward distance vector
        Vector3 forwardOffsetPosition = inputManager.hand2.transform.forward * (propDistanceToController);

        tweenToPosition = inputManager.hand2.transform.position + forwardOffsetPosition;
        tweenDistance = Vector3.Distance(transform.position, tweenToPosition) * Time.deltaTime;
        //--easing created by "tweenDistance" -a larger tweenDistance will make a faster tween
        transform.position = Vector3.MoveTowards(transform.position, tweenToPosition, tweenDistance * tweenSpeed);
    }
}