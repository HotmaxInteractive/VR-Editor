using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionControl : MonoBehaviour
{
    public objectSelect objSelect;
    editStateController stateController;

    private void Start()
    {
        stateController = GetComponent<editStateController>();
    }

    private void OnEnable()
    {
        stateController.behaviorName = "Position";
    }

    void Update()
    {
        if (objSelect.trackedController2.triggerPressed)
        {
            transform.position = objSelect.endPosition;

            //laser length will equal the distance from the hand to the raycast.point (endpoint)
            //objSelect.laserMaxLength = Vector3.Distance(objSelect.endPosition, objSelect.hand2.transform.position);

            /*
            if (Input.GetAxis("Vertical") < 1 && Input.GetAxis("Vertical") > .001f)
            {
                objSelect.laserMaxLength += 1f;
            }
            else if(Input.GetAxis("Vertical") > -1 && Input.GetAxis("Vertical") < -.001f)
            {
                objSelect.laserMaxLength -= 1f;
            }



         if (Input.GetAxis("leftPadHorizontal") != 0 || Input.GetAxis("rightPadHorizontal") != 0 || Input.GetAxis("leftPadVertical") != 0 || Input.GetAxis("rightPadVertical") != 0)
            {
                print(Input.GetAxis("leftPadHorizontal") + "   : left pad Horz");
                print(Input.GetAxis("rightPadHorizontal") + "   : right pad Horz");
                print(Input.GetAxis("leftPadVertical") + "   : left pad Vert");
                print(Input.GetAxis("rightPadVertical") + "   : right pad Vert");

            }
            */





        }
    }
}
