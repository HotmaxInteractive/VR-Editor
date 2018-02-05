using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class syncingAssetsDemo : MonoBehaviour
{
    public GameObject syncingAssetsText;
    public GameObject propInMenu;

    void Start()
    {
        inputManager.trackedController2.MenuButtonClicked += menuButtonClicked;
    }

    private void OnApplicationQuit()
    {
        inputManager.trackedController2.MenuButtonClicked -= menuButtonClicked;
    }

    void menuButtonClicked(object sender, ClickedEventArgs e)
    {
        syncingAssetsText.SetActive(true);
        Invoke("showAssetInMenu", 2);
    }

    void showAssetInMenu()
    {
        syncingAssetsText.SetActive(false);
        propInMenu.SetActive(true);
    }
}
