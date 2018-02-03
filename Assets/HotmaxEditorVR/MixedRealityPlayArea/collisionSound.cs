using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionSound : MonoBehaviour
{

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(audioSource)
        {
            if(collision.relativeVelocity.magnitude > 2)
            {
                audioSource.Play();
            }
        }
    }
}
