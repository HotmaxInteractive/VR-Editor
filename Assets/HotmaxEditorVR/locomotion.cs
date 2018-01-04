using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locomotion : MonoBehaviour
{
    private stateManager _stateManagerMutatorRef;

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
        _stateManagerMutatorRef = GameObject.FindObjectOfType(typeof(stateManager)) as stateManager;

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
        //--set initial "last position"
        lastPosition = handRB.gameObject.transform.localPosition;

        isGripped = true;

        _stateManagerMutatorRef.SET_PLAYER_IS_LOCOMOTING(true);
    }

    void ungripped(object sender, ClickedEventArgs e)
    {
        isGripped = false;

        _stateManagerMutatorRef.SET_PLAYER_IS_LOCOMOTING(false);
    }

    void Update()
    {   
        if(isGripped)
        {
            //--finds vector direction of the controller when gripped
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
