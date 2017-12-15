using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handColliderFollow : MonoBehaviour
{
    //--local refs
    private stateManager _stateManagerMutatorRef;

    private GameObject collidedWithHand;

    Transform selectorHand;

    private bool isCurrentlyColliding = false;

    private void Awake()
    {
        _stateManagerMutatorRef = GameObject.FindObjectOfType(typeof(stateManager)) as stateManager;
        selectorHand = GameObject.Find("Hand2").transform;
    }

    void Update()
    {
        transform.position = selectorHand.position;
        transform.rotation = selectorHand.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MonoBehaviour>() is prop)
        {
            if(!isCurrentlyColliding)
            {
                collidedWithHand = other.gameObject;
                _stateManagerMutatorRef.SET_OBJECT_COLLIDED_WITH_HAND(other.gameObject);
                isCurrentlyColliding = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<MonoBehaviour>() is prop)
        {
            if(collidedWithHand == other.gameObject)
            {
                _stateManagerMutatorRef.SET_OBJECT_COLLIDED_WITH_HAND(null);
                isCurrentlyColliding = false;
            }
        }
    }
}
