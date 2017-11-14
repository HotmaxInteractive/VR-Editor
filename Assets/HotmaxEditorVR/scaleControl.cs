using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleControl : MonoBehaviour
{
    public objectSelect objSelect;
    editStateController stateController;
    public float scaleSize = .5f;

    private void Start()
    {
        stateController = GetComponent<editStateController>();
    }

    private void OnEnable()
    {
        stateController.behaviorName = "Scale";
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.JoystickButton9) && Input.GetAxis("rightPadHorizontal") > .25f)
        {
            transform.localScale = new Vector3(transform.localScale.x + scaleSize, transform.localScale.y, transform.localScale.z);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton9) && Input.GetAxis("rightPadHorizontal") < -.25f)
        {
            transform.localScale = new Vector3(transform.localScale.x - scaleSize, transform.localScale.y, transform.localScale.z);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton9) && Input.GetAxis("rightPadVertical") > .25f)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + scaleSize, transform.localScale.z);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton9) && Input.GetAxis("rightPadVertical") < -.25f)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - scaleSize, transform.localScale.z);
        }


    }
}
