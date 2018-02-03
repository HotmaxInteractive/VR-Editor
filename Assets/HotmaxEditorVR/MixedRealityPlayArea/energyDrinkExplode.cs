using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyDrinkExplode : MonoBehaviour
{

    public GameObject fizzyPS;

    void Start()
    {
        inputManager.trackedController2.Gripped += gripped;
    }

    private void OnApplicationQuit()
    {
        inputManager.trackedController2.Gripped -= gripped;

    }

    void gripped(object sender, ClickedEventArgs e)
    {
        fizzyPS.SetActive(true);
        Invoke("setFizzInactive", 4);
    }

    void setFizzInactive()
    {
        fizzyPS.SetActive(false);
    }
}
