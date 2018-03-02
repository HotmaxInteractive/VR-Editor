using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuManager : MonoBehaviour
{
    //--local refs
    private stateManager.editorModes _editorMode = stateManager.editorMode;
    private GameObject _vrCamera;

    private void Start()
    {
        stateManager.editorModeEvent += updateEditorMode;
        _vrCamera = init.vrCamera;
    }

    private void OnApplicationQuit()
    {
        stateManager.editorModeEvent -= updateEditorMode;
    }

    void updateEditorMode(stateManager.editorModes value)
    {
        _editorMode = value;

        if (_editorMode == stateManager.editorModes.openMenuMode)
        {
            toggleMenuComponents(true);
        }
        else
        {
            toggleMenuComponents(false);
        }
    }

    public void toggleMenuComponents(bool on)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(on);
        }

        if (on)
        {
            setMenuToFacePlayer();
        }
    }

    void setMenuToFacePlayer()
    {
        //--make menu face the HMD
        Vector3 forwardAmount = _vrCamera.transform.position + (_vrCamera.transform.forward * 2.15f);
        Quaternion rotationAmount = _vrCamera.transform.rotation;

        transform.position = new Vector3(forwardAmount.x, _vrCamera.transform.position.y - 1, forwardAmount.z);

        transform.rotation = rotationAmount;
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);
    }
}

