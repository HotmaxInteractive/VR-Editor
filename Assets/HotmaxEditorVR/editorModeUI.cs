using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editorModeUI : MonoBehaviour
{
    private stateManager.activeQuadrants _activeQuadrant = stateManager.activeQuadrant;

    private void OnEnable()
    {
        stateManager.activeQuadrantEvent += updateactiveQuadrant;
        inputManager.trackedController2.PadUntouched += padUntouched;
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
            child.gameObject.SetActive(false);
        }

        switch (_activeQuadrant)
        {
            case stateManager.activeQuadrants.quarant0:
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            case stateManager.activeQuadrants.quarant1:
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            case stateManager.activeQuadrants.quarant2:
                transform.GetChild(2).gameObject.SetActive(true);
                break;
            case stateManager.activeQuadrants.quarant3:
                transform.GetChild(3).gameObject.SetActive(true);
                break;
        }
    }

    //--TODO: maybe this should go somewhere more explicit like a modeManager
    void padUntouched(object sender, ClickedEventArgs e)
    {
        switch (_activeQuadrant)
        {
            case stateManager.activeQuadrants.quarant0:
                init._stateManagerMutatorRef.SET_EDITOR_MODE_UNIVERSAL();
                break;
            case stateManager.activeQuadrants.quarant1:
                init._stateManagerMutatorRef.SET_EDITOR_MODE_CLONE_DELETE();
                break;
            case stateManager.activeQuadrants.quarant2:
                init._stateManagerMutatorRef.SET_EDITOR_MODE_OPEN_MENU();
                break;
            case stateManager.activeQuadrants.quarant3:
                init._stateManagerMutatorRef.SET_EDITOR_MODE_OPEN_MENU();
                break;
        }
    }
}
