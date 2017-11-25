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


    public static rotationModes rotationMode;
    public enum rotationModes
    {
        xRotationMode,
        yRotationMode,
        zRotationMode
    }
    public delegate void rotationModeHandler(rotationModes value);
    public static event rotationModeHandler rotationModeEvent;




    public static GameObject selectedObject;
    public delegate void selectedObjectHandler(GameObject obj);
    public static event selectedObjectHandler selectedObjectEvent;



    public static bool selectedObjectIsActive = false;

    public delegate void selectedObjectIsActiveHandler(bool value);
    public static event selectedObjectIsActiveHandler selectedObjectIsActiveEvent;


    public static bool rotationGizmosActive = false;

    public delegate void rotationGizmoIsActiveHandler(bool value);
    public static event rotationGizmoIsActiveHandler rotationGizmosActiveEvent;

    public static bool rotationGizmoIsSelected = false;

    public delegate void rotationGizmoIsSelectedHandler(bool value);
    public static event rotationGizmoIsSelectedHandler rotationGizmoIsSelectedEvent;

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

    public void SET_ROTATION_GIZMOS_ACTIVE(bool value)
    {
        rotationGizmosActive = value;
        rotationGizmosActiveEvent(value);
    }

    public void SET_ROTATION_GIZMO_IS_SELECTED(bool value)
    {
        rotationGizmoIsSelected = value;
        rotationGizmoIsSelectedEvent(value);
    }

    public void SET_X_ROTATION_GIZMO_ACTIVE()
    {
        rotationMode = rotationModes.xRotationMode;
        rotationModeEvent(rotationModes.xRotationMode);
        print(rotationMode);
    }

    public void SET_Y_ROTATION_GIZMO_ACTIVE()
    {
        rotationMode = rotationModes.yRotationMode;
        rotationModeEvent(rotationModes.yRotationMode);
        print(rotationMode);
    }

    public void SET_Z_ROTATION_GIZMO_ACTIVE()
    {
        rotationMode = rotationModes.zRotationMode;
        rotationModeEvent(rotationModes.zRotationMode);
        print(rotationMode);
    }
}
