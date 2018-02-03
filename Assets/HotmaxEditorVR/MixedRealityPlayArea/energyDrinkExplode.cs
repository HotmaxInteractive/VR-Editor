using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyDrinkExplode : MonoBehaviour
{

    public GameObject fizzyPS;
    private AudioSource audioSource;

    void Start()
    {
        inputManager.trackedController2.Gripped += gripped;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnApplicationQuit()
    {
        inputManager.trackedController2.Gripped -= gripped;

    }

    void gripped(object sender, ClickedEventArgs e)
    {
        audioSource.Play();
        Invoke("setFizzActive", 1);
    }

    void setFizzActive()
    {
        fizzyPS.SetActive(true);
        Invoke("setFizzInactive", 6);
    }

    void setFizzInactive()
    {
        fizzyPS.SetActive(false);
    }
}
