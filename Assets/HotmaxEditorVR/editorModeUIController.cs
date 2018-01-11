using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editorModeUIController : MonoBehaviour
{
    [SerializeField]
    public GameObject editorModeUI;

    void Start()
    {
        transform.parent = inputManager.hand2.transform;
        transform.localPosition = Vector3.zero;

        inputManager.trackedController2.PadTouched += padTouched;
        inputManager.trackedController2.PadUntouched += padUntouched;
    }

    private void OnApplicationQuit()
    {
        inputManager.trackedController2.PadTouched -= padTouched;
        inputManager.trackedController2.PadUntouched -= padUntouched;
    }

    void padTouched(object sender, ClickedEventArgs e)
    {
        editorModeUI.SetActive(true);
    }

    void padUntouched(object sender, ClickedEventArgs e)
    {
        editorModeUI.SetActive(false);
    }
}
