using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init : MonoBehaviour {

    //-- reference hooks
    //-- used for classes that frequently get destroyed but need reference to a gameObject
    public static GameObject rotationGizmos;
    public static GameObject props;

    void Awake ()
    {
        rotationGizmos = GameObject.Find("rotationGizmos");
        props = GameObject.Find("Props");
    }
}
