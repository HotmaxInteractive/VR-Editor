using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastSelect : MonoBehaviour
{
    //--laser stuff
    private Vector3 laserEndPosition;
    private LineRenderer laserLineRenderer;
    [SerializeField]
    private float laserWidth = 0.01f;
    [SerializeField]
    private float laserMaxLength = 5f;
    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;

    //--local refs
    private GameObject _objectCollidedWithHand;
    private stateManager _stateManagerMutatorRef;
    private Transform _hand2;
    private SteamVR_TrackedController _trackedController2;

    private void Awake()
    {
        _stateManagerMutatorRef = init._stateManagerMutatorRef;
        _hand2 = inputManager.hand2.gameObject.transform;
        _trackedController2 = inputManager.trackedController2;

        laserLineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        _trackedController2.TriggerClicked += triggerClicked;
        _trackedController2.TriggerUnclicked += triggerUnclicked;
        stateManager.objectCollidedWithHandEvent += updateObjectCollidedWithHand;
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;

        //Initializing the helper laser for the selection tool. 
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.endWidth = laserWidth;
    }

    private void OnApplicationQuit()
    {
        _trackedController2.TriggerClicked -= triggerClicked;
        _trackedController2.TriggerUnclicked -= triggerUnclicked;
        stateManager.objectCollidedWithHandEvent -= updateObjectCollidedWithHand;
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        if (_objectCollidedWithHand == null)
        {
            select(_hand2.position, _hand2.forward);
        }
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        if (_objectCollidedWithHand == null)
        {
            _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(false);
        }
    }

    void updateObjectCollidedWithHand(GameObject value)
    {
        _objectCollidedWithHand = value;
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;

        if (_selectedObjectIsActive)
        {
            laserLineRenderer.enabled = false;
        }
        else
        {
            laserLineRenderer.enabled = true;
        }
    }

    void Update()
    {
        ShootLaserFromTargetPosition(_hand2.position, _hand2.forward, laserMaxLength);
    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit hit;
        laserEndPosition = targetPosition + (length * direction);

        if (Physics.Raycast(ray, out hit, length))
        {
            laserEndPosition = hit.point;
        }

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, laserEndPosition);
    }

    void select(Vector3 targetPosition, Vector3 direction)
    {
        RaycastHit hit;
        Ray ray = new Ray(targetPosition, direction);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<MonoBehaviour>() is IHittable)
            {
                //have hit "prop" instance fire receiveHit function
                hit.collider.gameObject.GetComponent<IHittable>().receiveHit(hit);
            }
        }
    }
}
