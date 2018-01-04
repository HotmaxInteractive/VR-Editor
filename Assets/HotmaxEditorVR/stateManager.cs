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

    public static bool playerIsLocomoting;
    public delegate void playerIsLocomotingHandler(bool value);
    public static event playerIsLocomotingHandler playerIsLocomotingEvent;

    public delegate void spawnPageDecrementedHandler();
    public static event spawnPageDecrementedHandler spawnPageIncrementedEvent;

    public delegate void spawnPageIncrementedHandler();
    public static event spawnPageIncrementedHandler spawnPageDecrementedEvent;

    public delegate void closeMenuHandler();
    public static event closeMenuHandler closeMenuEvent;

    public delegate void materialPageDecrementedHandler();
    public static event materialPageDecrementedHandler materialPageIncrementedEvent;

    public delegate void materialPageIncrementedHandler();
    public static event materialPageIncrementedHandler materialPageDecrementedEvent;

    public delegate void massScaleHitHandler();
    public static event massScaleHitHandler massScaleHitEvent;

    public static Vector3 raycastHitPoint;
    public static GameObject raycastHitGameObject;
    public delegate void raycastHitInfoHandler(Vector3 value1, GameObject value2);
    public static event raycastHitInfoHandler raycastHitInfoEvent;

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

    public void SET_PLAYER_IS_LOCOMOTING(bool value)
    {
        playerIsLocomoting = value;
        if (playerIsLocomotingEvent != null)
        {
            playerIsLocomotingEvent(value);
        }
    }

    public void SET_SPAWN_PAGE_INCREMENTED()
    {
        if (spawnPageIncrementedEvent != null)
        {
            spawnPageIncrementedEvent();
        }
    }

    public void SET_SPAWN_PAGE_DECREMENTED()
    {
        if (spawnPageDecrementedEvent != null)
        {
            spawnPageDecrementedEvent();
        }
    }

    public void SET_MENU_CLOSED()
    {
        if (closeMenuEvent != null)
        {
            closeMenuEvent();
        }
    }

    public void SET_MATERIAL_PAGE_INCREMENTED()
    {
        if (materialPageIncrementedEvent != null)
        {
            materialPageIncrementedEvent();
        }
    }

    public void SET_MATERIAL_PAGE_DECREMENTED()
    {
        if (materialPageDecrementedEvent != null)
        {
            materialPageDecrementedEvent();
        }
    }

    public void SET_MASS_SCALE_HIT()
    {
        if (massScaleHitEvent != null)
        {
            massScaleHitEvent();
        }
    }

    public void SET_RAYCAST_HIT_INFO(Vector3 value1, GameObject value2)
    {
        if (raycastHitInfoEvent != null)
        {
            raycastHitInfoEvent(value1, value2);
        }
    }
}
