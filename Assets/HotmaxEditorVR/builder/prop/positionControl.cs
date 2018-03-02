using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionControl : MonoBehaviour
{
    //--local refs
    private GameObject _objectCollidedWithHand;
    private Transform _raycastPoint;
    private Transform _hand2;
    private GameObject _rotationGizmos;
    private GameObject _props;
    private SteamVR_TrackedController _trackedController2;
    private SteamVR_Controller.Device _selectorHand;

    private float initialScrollYPosition;
    private float currentScrollYPos;

    private float initialDistanceToController;
    private float scrollDistance = 0;

    [SerializeField]
    private float scrollSpeed = .2f;
    [SerializeField]
    private float tweenSpeed = 4;
    private float tweenDistance;

    private Vector3 forwardOffsetPosition;
    private Vector3 tweenToPosition;

    private void OnEnable()
    {
        _objectCollidedWithHand = stateManager.objectCollidedWithHand;
        _raycastPoint = init.raycastPoint.transform;
        _hand2 = inputManager.hand2.transform;
        _rotationGizmos = init.rotationGizmos;
        _props = init.props;
        _trackedController2 = inputManager.trackedController2;
        _selectorHand = inputManager.selectorHand;

        stateManager.objectCollidedWithHandEvent += updateObjectCollidedWithHand;

        initialScrollYPosition = _selectorHand.GetAxis().y;
        initialDistanceToController = Vector3.Distance(transform.position, _hand2.transform.position);
        scrollDistance = 0;
        _rotationGizmos.SetActive(false);

        //--initialize raycastPoint
        forwardOffsetPosition = _hand2.forward * (initialDistanceToController + scrollDistance);
        tweenToPosition = _hand2.position + forwardOffsetPosition;
        _raycastPoint.position = tweenToPosition;
    }

    private void OnDisable()
    {
        stateManager.objectCollidedWithHandEvent -= updateObjectCollidedWithHand;
        transform.parent = _props.transform;
    }

    void updateObjectCollidedWithHand(GameObject value)
    {
        _objectCollidedWithHand = value;
    }

    void Update()
    {
        currentScrollYPos = _selectorHand.GetAxis().y;

        if (_objectCollidedWithHand == gameObject)
        {
            if(_trackedController2.padTouched)
            {
                propOffsetController(true);
                parentPropInBall();
                unparentBall();
            }
            else
            {
                parentBallInHand();
                parentPropInBall();
            }
            return;
        }
        else
        {
            propOffsetController(false);
            parentPropInBall();
            unparentBall();
        }
    }

    void parentBallInHand()
    {
        if (_raycastPoint.parent != _hand2)
        {
            _raycastPoint.parent = _hand2;
        }
    }

    void parentPropInBall()
    {
        if (transform.parent != _raycastPoint)
        {
            transform.parent = _raycastPoint;
        }
    }

    void unparentBall()
    {
        _raycastPoint.parent = null;
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
        forwardOffsetPosition = _hand2.forward * (propDistanceToController);

        tweenToPosition = _hand2.position + forwardOffsetPosition;
        tweenDistance = Vector3.Distance(_raycastPoint.position, tweenToPosition) * Time.deltaTime;

        //--easing created by "tweenDistance" -a larger tweenDistance will make a faster tween
        _raycastPoint.position = Vector3.MoveTowards(_raycastPoint.position, tweenToPosition, tweenDistance * tweenSpeed);
    }
}