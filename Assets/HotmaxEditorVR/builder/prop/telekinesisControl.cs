using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: name positionControl
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

    private GameObject _objectCollidedWithHand = stateManager.objectCollidedWithHand;
    private Transform initialParent;

    private void OnEnable()
    {
        if (_objectCollidedWithHand == gameObject)
        {
            //handle grab movement
            print(_objectCollidedWithHand);
        }

        stateManager.objectCollidedWithHandEvent += updateObjectCollidedwithhand;

        initialScrollYPosition = inputManager.selectorHand.GetAxis().y;
        distToController = Vector3.Distance(transform.position, inputManager.hand2.transform.position);
        scrollDistance = 0;
        init.rotationGizmos.SetActive(false);
    }

    private void OnDisable()
    {
        stateManager.objectCollidedWithHandEvent -= updateObjectCollidedwithhand;
        transform.parent = init.props.transform;
    }

    void updateObjectCollidedwithhand(GameObject value)
    {
        _objectCollidedWithHand = value;
    }

    void Update()
    {
        if (_objectCollidedWithHand == gameObject)
        {
            //handle grab movement
            parentPropInHand();
            return;
        }
        else
        {
            tweenDistance = Vector3.Distance(transform.position, tweenToPosition) * Time.deltaTime;
            currentScrollYPos = inputManager.selectorHand.GetAxis().y;

            //--Creates a local controller forward distance vector
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

    void parentPropInHand()
    {
        if (transform.parent != inputManager.hand2.transform)
        {
            transform.parent = inputManager.hand2.transform;
        }
    }
}