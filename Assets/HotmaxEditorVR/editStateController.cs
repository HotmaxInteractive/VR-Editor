using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editStateController : MonoBehaviour
{
    positionControl posControl;
    rotationControl rotControl;
    scaleControl scalControl;
    cloneControl cloneControl;

    public objectSelect objSelect;

    public string behaviorName = "";

    public List<MonoBehaviour> components = new List<MonoBehaviour>();

    Collider col;

    int stateNumber = 0;

    void Start()
    {
        posControl = GetComponent<positionControl>();
        rotControl = GetComponent<rotationControl>();
        scalControl = GetComponent<scaleControl>();
        cloneControl = GetComponent<cloneControl>();

        components.Add(posControl);
        components.Add(rotControl);
        components.Add(scalControl);
        components.Add(cloneControl);

        enableState(stateNumber);
    }

    void Update()
    {
        incrementState();
    }

    void incrementState()
    {
        if (Input.GetButtonDown("menuRight"))
        {
            if (stateNumber < components.Count - 1)
            {
                stateNumber += 1;
            }
            else
            {
                stateNumber = 0;
            }
            enableState(stateNumber);
        }
    }

    void enableState(int state)
    {
        for(int i = 0; i < components.Count; i ++)
        {
            components[i].enabled = false;
        }
        components[state].enabled = true;

        objSelect.hand2.GetComponent<TextMesh>().text = behaviorName;
    }
}
