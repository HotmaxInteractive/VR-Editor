using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locomotion : MonoBehaviour
{
    private bool isGripped = false;

    private Vector3 lastPosition;
    private Vector3 currentControllerPosition;

    [SerializeField]
    private float scrollSpeed = .2f;
    private float scrollDistance = 0;

    private GameObject player;
    private Rigidbody handRB;


    void Start()
    {
        inputManager.trackedController2.Gripped += gripped;
        inputManager.trackedController2.Ungripped += ungripped;

        player = GameObject.Find("Player");
        handRB = GameObject.Find("handColliderFollow").GetComponent<Rigidbody>();
    }

    private void OnApplicationQuit()
    {
        inputManager.trackedController2.Gripped -= gripped;
        inputManager.trackedController2.Ungripped -= ungripped;
    }

    void gripped(object sender, ClickedEventArgs e)
    {
        currentControllerPosition = handRB.gameObject.transform.localPosition;
        lastPosition = handRB.gameObject.transform.localPosition;

        isGripped = true;
    }

    void ungripped(object sender, ClickedEventArgs e)
    {
        isGripped = false;
    }

    void Update()
    {   
        if(isGripped)
        {
            currentControllerPosition = handRB.transform.localPosition;
            var direction = currentControllerPosition - lastPosition;
            var localDirection = handRB.transform.TransformDirection(direction);
            lastPosition = currentControllerPosition;

            player.transform.Translate(-localDirection * 5);

        }
        else
        {
            return;
        }
    }
}
