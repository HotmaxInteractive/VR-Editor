using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateManager : MonoBehaviour
{
    public static editorModes editorMode = editorModes.universalTransformMode;
    public enum editorModes
    {
        universalTransformMode,
        cloneDeleteMode,
        openMenuMode
    }
    public delegate void editorModeHandler(editorModes value);
    public static event editorModeHandler editorModeEvent;




    public static GameObject selectedObject;
    public delegate void selectedObjectHandler(GameObject obj);
    public static event selectedObjectHandler selectedObjectEvent;



    public static bool selectedObjectIsActive = false;

    public delegate void selectedObjectIsActiveHandler(bool value);
    public static event selectedObjectIsActiveHandler selectedObjectIsActiveEvent;

    //------------------MUTATORS------------------\\

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
    public void SET_SELECTED_OBJECT(GameObject value)
    {
        selectedObject = value;
        selectedObjectEvent(value);
    }

    public void SET_SELECTED_OBJECT_IS_ACTIVE(bool value)
    {
        selectedObjectIsActive = value;
        selectedObjectIsActiveEvent(value);
    }
}
