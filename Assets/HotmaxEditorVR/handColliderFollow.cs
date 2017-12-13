using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handColliderFollow : MonoBehaviour
{
    //--local refs
    private stateManager _stateManagerMutatorRef;

    Transform selectorHand;

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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<MonoBehaviour>() is IHittable)
        {
            _stateManagerMutatorRef.SET_OBJECT_COLLIDED_WITH_HAND(other.gameObject);
        }
    }
}
