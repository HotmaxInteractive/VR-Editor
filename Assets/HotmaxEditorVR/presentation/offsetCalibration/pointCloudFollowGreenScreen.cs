using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointCloudFollowGreenScreen : MonoBehaviour
{
    private Transform zedGreenScreen;

	void Start ()
    {
        zedGreenScreen = FindObjectOfType<GreenScreenManager>().transform;
	}
	
	void Update ()
    {
        transform.position = zedGreenScreen.position;
        transform.rotation = zedGreenScreen.rotation;
	}
}
