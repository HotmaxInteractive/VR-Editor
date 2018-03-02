using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init : MonoBehaviour {

    //-- reference hooks
    //-- used for classes that frequently get destroyed but need reference to a gameObject
    public static GameObject rotationGizmos;
    public static GameObject props;
    public static GameObject deletePanel;
    public static GameObject deletedProps;
    public static GameObject vrCamera;
    public static GameObject scaleController;
    public static GameObject raycastPoint;

    public static stateManager _stateManagerMutatorRef;

    void Awake ()
    {
        rotationGizmos = GameObject.Find("rotationGizmos");
        props = GameObject.Find("props");
        deletePanel = GameObject.Find("deletePanel");
        deletedProps = GameObject.Find("deletedProps");
        vrCamera = GameObject.Find("VRCamera");
        scaleController = GameObject.Find("scaleController");
        raycastPoint = GameObject.Find("raycastPoint");

        _stateManagerMutatorRef = FindObjectOfType(typeof(stateManager)) as stateManager;
    }
}
