using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class editStateController : MonoBehaviour
{
    universalTransform _universalTransform;
    scaleControl _scaleControl;
    cloneControl _cloneControl;

    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

    public objectSelect objSelect;

    string behaviorName = "";

    public List<MonoBehaviour> components = new List<MonoBehaviour>();

    int stateNumber = 0;

    private stateManager.editorModes _editorMode = stateManager.editorMode;

    private void Awake()
    {
        stateManager.editorModeEvent += updateEditorMode;
    }

    protected virtual void OnApplicationQuit()
    {
        stateManager.editorModeEvent -= updateEditorMode;
    }

    void updateEditorMode(stateManager.editorModes value)
    {
        _editorMode = value;
        setEditorMode();
    }

    void Start()
    {
        trackedObject = objSelect.hand2.GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);

        _universalTransform = GetComponent<universalTransform>();
        _scaleControl = GetComponent<scaleControl>();
        _cloneControl = GetComponent<cloneControl>();

        components.Add(_universalTransform);
        components.Add(_scaleControl);
        components.Add(_cloneControl);

        setEditorMode();
    }

 
    void setEditorMode()
    {
        if (_editorMode == stateManager.editorModes.universalTransformMode)
        {
            enableEditorState(0);
            behaviorName = "Position (to be universal transform)";
        }
        if (_editorMode == stateManager.editorModes.cloneDeleteMode)
        {
            enableEditorState(1);
            behaviorName = "scale (to be clone delete)";
        }
        if (_editorMode == stateManager.editorModes.openMenuMode)
        {
            enableEditorState(2);
            behaviorName = "Clone (to be open menu)";
        }
    }

    void enableEditorState(int state)
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].enabled = false;
        }
        components[state].enabled = true;

        objSelect.hand2.GetComponent<TextMesh>().text = behaviorName;
    }
}
