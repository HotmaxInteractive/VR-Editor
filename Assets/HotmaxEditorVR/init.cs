using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init : MonoBehaviour {

    //-- reference hooks
    //-- used for classes that frequently get destroyed but need reference to a gameObject
    public static GameObject rotationGizmos;
    public static GameObject props;

    public static stateManager _stateManagerMutatorRef;

    void Awake ()
    {
        rotationGizmos = GameObject.Find("rotationGizmos");
        props = GameObject.Find("Props");
        _stateManagerMutatorRef = GameObject.FindObjectOfType(typeof(stateManager)) as stateManager;
    }
}
