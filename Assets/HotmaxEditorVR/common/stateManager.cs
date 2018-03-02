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
        openMenuMode,
    }
    public delegate void editorModeHandler(editorModes value);
    public static event editorModeHandler editorModeEvent;

    public static bool rotationGizmoIsSelected = false;
    public delegate void rotationGizmoIsSelectedHandler(bool value);
    public static event rotationGizmoIsSelectedHandler rotationGizmoIsSelectedEvent;

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

    public static GameObject objectCollidedWithHand;
    public delegate void objectCollidedWithHandHandler(GameObject value);
    public static event objectCollidedWithHandHandler objectCollidedWithHandEvent;

    //------------------MUTATORS------------------\\

    public void SET_EDITOR_MODE_UNIVERSAL()
    {
        editorMode = editorModes.universalTransformMode;
        if (editorModeEvent != null)
        {
            editorModeEvent(editorModes.universalTransformMode);
        }
    }
    public void SET_EDITOR_MODE_CLONE_DELETE()
    {
        editorMode = editorModes.cloneDeleteMode;
        if (editorModeEvent != null)
        {
            editorModeEvent(editorModes.cloneDeleteMode);
        }
    }

    public void SET_EDITOR_MODE_OPEN_MENU()
    {
        editorMode = editorModes.openMenuMode;
        if (editorModeEvent != null)
        {
            editorModeEvent(editorModes.openMenuMode);
        }
    }

    public void SET_SELECTED_OBJECT(GameObject value)
    {
        selectedObject = value;
        if (selectedObjectEvent != null)
        {
            selectedObjectEvent(value);
        }
    }

    public void SET_SELECTED_OBJECT_IS_ACTIVE(bool value)
    {
        selectedObjectIsActive = value;
        if (selectedObjectIsActiveEvent != null)
        {
            selectedObjectIsActiveEvent(value);
        }
    }

    public void SET_ROTATION_GIZMO_IS_SELECTED(bool value)
    {
        rotationGizmoIsSelected = value;
        if (rotationGizmoIsSelectedEvent != null)
        {
            rotationGizmoIsSelectedEvent(value);
        }
    }

    public void SET_X_ROTATION_GIZMO_ACTIVE()
    {
        rotationMode = rotationModes.xRotationMode;
        if (rotationModeEvent != null)
        {
            rotationModeEvent(rotationModes.xRotationMode);
        }
    }

    public void SET_Y_ROTATION_GIZMO_ACTIVE()
    {
        rotationMode = rotationModes.yRotationMode;
        if (rotationModeEvent != null)
        {
            rotationModeEvent(rotationModes.yRotationMode);
        }
    }

    public void SET_Z_ROTATION_GIZMO_ACTIVE()
    {
        rotationMode = rotationModes.zRotationMode;
        if (rotationModeEvent != null)
        {
            rotationModeEvent(rotationModes.zRotationMode);
        }
    }

    public void SET_OBJECT_COLLIDED_WITH_HAND(GameObject value)
    {
        objectCollidedWithHand = value;
        if (objectCollidedWithHandEvent != null)
        {
            objectCollidedWithHandEvent(value);
        }
    }
}
