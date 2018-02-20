using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spectatorCameraInit : MonoBehaviour
{
	void Start ()
    {
        transform.parent = init.player.transform;
	}
}
