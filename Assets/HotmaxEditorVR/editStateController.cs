using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class editStateController : MonoBehaviour
{
    positionControl posControl;
    rotationControl rotControl;
    scaleControl scalControl;
    cloneControl cloneControl;

    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

    public objectSelect objSelect;

    public string behaviorName = "";

    public List<MonoBehaviour> components = new List<MonoBehaviour>();

    Collider col;

    int stateNumber = 0;
    
    void setControllerIndex()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
    }

    void Start()
    {
        trackedObject = objSelect.hand2.GetComponent<SteamVR_TrackedObject>();
        setControllerIndex();

        posControl = GetComponent<positionControl>();
        rotControl = GetComponent<rotationControl>();
        scalControl = GetComponent<scaleControl>();
        cloneControl = GetComponent<cloneControl>();

        components.Add(posControl);
        components.Add(rotControl);
        components.Add(scalControl);
        components.Add(cloneControl);

        enableEditorState(stateNumber);

    }

    void Update()
    {
        if (device.GetAxis().x != 0 || device.GetAxis().y != 0)
        {
            if (device.GetAxis().x < 0 && device.GetAxis().y > 0)
            {
                enableEditorState(0);
                behaviorName = "Position";
            }
            if (device.GetAxis().x > 0 && device.GetAxis().y > 0)
            {
                enableEditorState(1);
                behaviorName = "Rotation";
            }
            if (device.GetAxis().x < 0 && device.GetAxis().y < 0)
            {
                enableEditorState(2);
                behaviorName = "Scale";
            }
            if (device.GetAxis().x > 0 && device.GetAxis().y < 0)
            {
                enableEditorState(3);
                behaviorName = "Clone";
            }
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
