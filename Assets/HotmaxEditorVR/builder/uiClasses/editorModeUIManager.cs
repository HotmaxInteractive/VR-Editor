using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editorModeUIManager : MonoBehaviour
{
    //--local refs
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private stateManager.activeQuadrants _activeQuadrant = stateManager.activeQuadrant;

    [SerializeField]
    public GameObject editorModeUI;

    private void OnEnable()
    {
        removeHighlight();
    }

    void Start()
    {
        //--Gets reparented in the selector hand and set to the hands position
        transform.parent = inputManager.hand2.transform;
        transform.localPosition = Vector3.zero;

        inputManager.trackedController2.PadTouched += padTouched;
        inputManager.trackedController2.PadUntouched += padUntouched;

        stateManager.activeQuadrantEvent += updateActiveQuadrant;
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
    }

    private void OnApplicationQuit()
    {
        inputManager.trackedController2.PadTouched -= padTouched;
        inputManager.trackedController2.PadUntouched -= padUntouched;

        stateManager.activeQuadrantEvent -= updateActiveQuadrant;
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
    }

    void padTouched(object sender, ClickedEventArgs e)
    {
        //--don't show ui when prop is active
        if (!_selectedObjectIsActive)
        {
            editorModeUI.SetActive(true);
        }
    }

    void padUntouched(object sender, ClickedEventArgs e)
    {
        if (!_selectedObjectIsActive)
        {
            setEditorMode();
        }
        editorModeUI.SetActive(false);
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
    }

    void updateActiveQuadrant(stateManager.activeQuadrants value)
    {
        _activeQuadrant = value;

        removeHighlight();

        switch (_activeQuadrant)
        {
            case stateManager.activeQuadrants.topLeft:
                editorModeUI.transform.GetChild(0).gameObject.AddComponent<cakeslice.Outline>();
                break;
            case stateManager.activeQuadrants.topRight:
                editorModeUI.transform.GetChild(1).gameObject.AddComponent<cakeslice.Outline>();
                break;
            case stateManager.activeQuadrants.bottomRight:
                editorModeUI.transform.GetChild(2).gameObject.AddComponent<cakeslice.Outline>();
                break;
            case stateManager.activeQuadrants.bottomLeft:
                editorModeUI.transform.GetChild(3).gameObject.AddComponent<cakeslice.Outline>();
                break;
        }
    }

    void setEditorMode()
    {
        switch (_activeQuadrant)
        {
            case stateManager.activeQuadrants.topLeft:
                init._stateManagerMutatorRef.SET_EDITOR_MODE_UNIVERSAL();
                break;
            case stateManager.activeQuadrants.topRight:
                init._stateManagerMutatorRef.SET_EDITOR_MODE_CLONE_DELETE();
                break;
            case stateManager.activeQuadrants.bottomRight:
                init._stateManagerMutatorRef.SET_EDITOR_MODE_OPEN_MENU();
                break;
            case stateManager.activeQuadrants.bottomLeft:
                init._stateManagerMutatorRef.SET_EDITOR_MODE_OPEN_MENU();
                break;
        }
    }

    void removeHighlight()
    {
        foreach (Transform child in editorModeUI.transform)
        {
            Destroy(child.gameObject.GetComponent<cakeslice.Outline>());
        }
    }
}
