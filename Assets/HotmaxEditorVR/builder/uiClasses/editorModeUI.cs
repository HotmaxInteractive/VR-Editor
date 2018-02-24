using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editorModeUI : MonoBehaviour
{
    //--local refs
    private stateManager.activeQuadrants _activeQuadrant = stateManager.activeQuadrant;

    private void OnEnable()
    {
        stateManager.activeQuadrantEvent += updateactiveQuadrant;
        inputManager.trackedController2.PadUntouched += padUntouched;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject.GetComponent<cakeslice.Outline>());
        }
    }

    private void OnDisable()
    {
        stateManager.activeQuadrantEvent -= updateactiveQuadrant;
        inputManager.trackedController2.PadUntouched -= padUntouched;
    }

    void updateactiveQuadrant(stateManager.activeQuadrants value)
    {
        _activeQuadrant = value;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject.GetComponent<cakeslice.Outline>());
        }

        switch (_activeQuadrant)
        {
            case stateManager.activeQuadrants.topLeft:
                transform.GetChild(0).gameObject.AddComponent<cakeslice.Outline>();
                break;
            case stateManager.activeQuadrants.topRight:
                transform.GetChild(1).gameObject.AddComponent<cakeslice.Outline>();
                break;
            case stateManager.activeQuadrants.bottomRight:
                transform.GetChild(2).gameObject.AddComponent<cakeslice.Outline>();
                break;
            case stateManager.activeQuadrants.bottomLeft:
                transform.GetChild(3).gameObject.AddComponent<cakeslice.Outline>();
                break;
        }      
    }
    
    //--TODO: maybe this should go somewhere more explicit like a modeManager? it is a little strange that the UI is controlling modes
    void padUntouched(object sender, ClickedEventArgs e)
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
}
