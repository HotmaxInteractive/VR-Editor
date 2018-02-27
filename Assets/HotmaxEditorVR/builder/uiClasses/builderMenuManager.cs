using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class builderMenuManager : MonoBehaviour
{
    //--local refs
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private bool _rotationGizmoIsSelected = stateManager.rotationGizmoIsSelected;
    private float _padX;
    private float _padY;

    [SerializeField]
    public GameObject builderMenuUI;

    void Start()
    {
        //--Gets reparented in the selector hand and set to the hands position
        transform.parent = inputManager.hand2.transform;
        transform.localPosition = Vector3.zero;

        inputManager.trackedController2.PadTouched += padTouched;
        inputManager.trackedController2.PadUntouched += padUntouched;

        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
        stateManager.rotationGizmoIsSelectedEvent += updateRotationGizmoIsSelected;
    }

    private void OnApplicationQuit()
    {
        inputManager.trackedController2.PadTouched -= padTouched;
        inputManager.trackedController2.PadUntouched -= padUntouched;

        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
        stateManager.rotationGizmoIsSelectedEvent -= updateRotationGizmoIsSelected;
    }

    void padTouched(object sender, ClickedEventArgs e)
    {
        //--don't show ui when prop is active
        if (!_selectedObjectIsActive)
        {
            builderMenuUI.SetActive(true);
        }
    }

    void padUntouched(object sender, ClickedEventArgs e)
    {
        if (!_selectedObjectIsActive)
        {
            setEditorMode();
        }
        builderMenuUI.SetActive(false);
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
    }

    void updateRotationGizmoIsSelected(bool value)
    {
        _rotationGizmoIsSelected = value;
    }

    private void Update()
    {
        updatePadCoords();
    }

    void updatePadCoords()
    {
        if (inputManager.selectorHand != null)
        {
            if (_selectedObjectIsActive || _rotationGizmoIsSelected)
            {
                return;
            }
            else
            {
                if (inputManager.selectorHand.GetAxis().x != 0 || inputManager.selectorHand.GetAxis().y != 0)
                {
                    _padX = inputManager.selectorHand.GetAxis().x;
                    _padY = inputManager.selectorHand.GetAxis().y;
                }
            }
        }
    }

    void setEditorMode()
    {
        if (_padX < 0 && _padY > 0)
        {
            init._stateManagerMutatorRef.SET_EDITOR_MODE_UNIVERSAL();
        }
        if (_padX > 0 && _padY > 0)
        {
            init._stateManagerMutatorRef.SET_EDITOR_MODE_CLONE_DELETE();
        }
        if (_padX < 0 && _padY < 0)
        {
            init._stateManagerMutatorRef.SET_EDITOR_MODE_OPEN_MENU();
        }
        if (_padX > 0 && _padY < 0)
        {
            init._stateManagerMutatorRef.SET_EDITOR_MODE_OPEN_MENU();
        }
    }
}