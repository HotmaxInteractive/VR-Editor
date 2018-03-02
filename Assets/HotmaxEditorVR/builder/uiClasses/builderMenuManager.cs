using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class builderMenuManager : MonoBehaviour
{
    //--local refs
    private stateManager _stateManagerMutatorRef;
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;
    private bool _rotationGizmoIsSelected = stateManager.rotationGizmoIsSelected;
    private float _padX;
    private float _padY;
    private SteamVR_TrackedController _trackedController2;
    private Valve.VR.InteractionSystem.Hand _hand2;
    private SteamVR_Controller.Device _selectorHand;

    [SerializeField]
    public GameObject builderMenuUI;

    void Start()
    {
        _stateManagerMutatorRef = init._stateManagerMutatorRef;
        _trackedController2 = inputManager.trackedController2;
        _hand2 = inputManager.hand2;

        //--Gets reparented in the selector hand and set to the hands position
        transform.parent = _hand2.transform;
        transform.localPosition = Vector3.zero;

        _trackedController2.PadTouched += padTouched;
        _trackedController2.PadUntouched += padUntouched;

        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
        stateManager.rotationGizmoIsSelectedEvent += updateRotationGizmoIsSelected;
    }

    private void OnApplicationQuit()
    {
        _trackedController2.PadTouched -= padTouched;
        _trackedController2.PadUntouched -= padUntouched;

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
        _selectorHand = inputManager.selectorHand;

        if (_selectorHand != null)
        {
            if (_selectedObjectIsActive || _rotationGizmoIsSelected)
            {
                return;
            }
            else
            {
                if (_selectorHand.GetAxis().x != 0 || _selectorHand.GetAxis().y != 0)
                {
                    _padX = _selectorHand.GetAxis().x;
                    _padY = _selectorHand.GetAxis().y;
                }
            }
        }
    }

    void setEditorMode()
    {
        if (_padX < 0 && _padY > 0)
        {
            _stateManagerMutatorRef.SET_EDITOR_MODE_UNIVERSAL();
        }
        if (_padX > 0 && _padY > 0)
        {
            _stateManagerMutatorRef.SET_EDITOR_MODE_CLONE_DELETE();
        }
        if (_padX < 0 && _padY < 0)
        {
            _stateManagerMutatorRef.SET_EDITOR_MODE_OPEN_MENU();
        }
        if (_padX > 0 && _padY < 0)
        {
            _stateManagerMutatorRef.SET_EDITOR_MODE_OPEN_MENU();
        }
    }
}