using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateManager : MonoBehaviour
{
    public static inputPadModes inputPadMode = inputPadModes.quadrantMode;
    public enum inputPadModes
    {
        quadrantMode,
        tuningMode
    }
    public delegate void inputModeHandler(inputPadModes value);
    public static event inputModeHandler inputModeEvent;

    public static editorModes editorMode = editorModes.universalTransformMode;
    public enum editorModes
    {
        universalTransformMode,
        cloneDeleteMode,
        openMenuMode
    }
    public delegate void editorModeHandler(editorModes value);
    public static event editorModeHandler editorModeEvent;




    //------------------MUTATORS------------------\\
    public void SET_INPUT_MODE_QUADRANT()
    {
        inputPadMode = inputPadModes.quadrantMode;
        inputModeEvent(inputPadModes.quadrantMode);
    }
    public void SET_INPUT_MODE_TUNING()
    {
        inputPadMode = inputPadModes.tuningMode;
        inputModeEvent(inputPadModes.tuningMode);
    }

    public void SET_EDITOR_MODE_UNIVERSAL()
    {
        editorMode = editorModes.universalTransformMode;
        editorModeEvent(editorModes.universalTransformMode);
    }
    public void SET_EDITOR_MODE_CLONE_DELETE()
    {
        editorMode = editorModes.cloneDeleteMode;
        editorModeEvent(editorModes.cloneDeleteMode);
    }
    public void SET_EDITOR_MODE_OPEN_MENU()
    {
        editorMode = editorModes.openMenuMode;
        editorModeEvent(editorModes.openMenuMode);
    }
}
