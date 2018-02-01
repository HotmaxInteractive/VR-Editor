using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionSound : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if(GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
