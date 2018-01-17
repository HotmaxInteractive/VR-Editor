using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editorModeUIController : MonoBehaviour
{
    //--local refs
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;

    [SerializeField]
    public GameObject editorModeUI;

    void Start()
    {
        transform.parent = inputManager.hand2.transform;
        transform.localPosition = Vector3.zero;

        inputManager.trackedController2.PadTouched += padTouched;
        inputManager.trackedController2.PadUntouched += padUntouched;

        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
    }

    private void OnApplicationQuit()
    {
        inputManager.trackedController2.PadTouched -= padTouched;
        inputManager.trackedController2.PadUntouched -= padUntouched;

        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
    }

    void padTouched(object sender, ClickedEventArgs e)
    {
        if (!_selectedObjectIsActive)
        {
            editorModeUI.SetActive(true);
        }
    }

    void padUntouched(object sender, ClickedEventArgs e)
    {
        editorModeUI.SetActive(false);
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
    }
}
