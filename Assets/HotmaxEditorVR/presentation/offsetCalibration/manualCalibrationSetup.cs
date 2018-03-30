using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class manualCalibrationSetup : MonoBehaviour
{
    public Transform Zed_Greenscreen;
    private bool isRotation = true;
    public GameObject rotationText;
    public GameObject positionText;

    private float _padX;
    private float _padY;
    private SteamVR_TrackedController _trackedController2;
    private SteamVR_Controller.Device _selectorHand;

    private enum transformableAxis
    {
        x,
        xReverse,
        y,
        yReverse,
        z,
        zReverse,
        none
    }
    private transformableAxis currentTransformableAxis = transformableAxis.none;

    private bool transformBtnClicked = false;

    void Start()
    {
        _trackedController2 = inputManager.trackedController2;
        _trackedController2.TriggerUnclicked += triggerUnclicked;
    }

    void OnApplicationQuit()
    {
        _trackedController2.TriggerUnclicked -= triggerUnclicked;
    }

    private void Update()
    {
        if (transformBtnClicked)
        {
            switch (currentTransformableAxis)
            {
                case transformableAxis.x:
                    moveOnAxis(0, Vector3.right, Vector3.forward);
                    break;
                case transformableAxis.xReverse:
                    moveOnAxis(1, Vector3.left, Vector3.back);
                    break;
                case transformableAxis.y:
                    moveOnAxis(2, Vector3.up, Vector3.right);
                    break;
                case transformableAxis.yReverse:
                    moveOnAxis(3, Vector3.down, Vector3.left);
                    break;
                case transformableAxis.z:
                    moveOnAxis(4, Vector3.forward, Vector3.up);
                    break;
                case transformableAxis.zReverse:
                    moveOnAxis(5, Vector3.back, Vector3.down);
                    break;
            }
        }
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        transformBtnClicked = false;
        currentTransformableAxis = transformableAxis.none;
        CancelInvoke();
    }


    public void moveX()
    {
        moveOnAxis(0, Vector3.right, Vector3.forward);
    }

    public void moveXReverse()
    {
        moveOnAxis(1, Vector3.left, Vector3.back);
    }

    public void moveY()
    {
        moveOnAxis(2, Vector3.up, Vector3.right);
    }

    public void moveYReverse()
    {
        moveOnAxis(3, Vector3.down, Vector3.left);
    }

    public void moveZ()
    {
        moveOnAxis(4, Vector3.forward, Vector3.up);
    }

    public void moveZReverse()
    {
        moveOnAxis(5, Vector3.back, Vector3.down);
    }

    public void toggleTransformControl()
    {
        isRotation = !isRotation;

        rotationText.SetActive(false);
        positionText.SetActive(false);

        if (isRotation)
        {
            rotationText.SetActive(true);
        }
        else
        {
            positionText.SetActive(true);
        }
    }

    void updatePadCoords()
    {
        _selectorHand = inputManager.selectorHand;

        if (_selectorHand.GetAxis().x != 0 || _selectorHand.GetAxis().y != 0)
        {
            _padX = _selectorHand.GetAxis().x;
            _padY = _selectorHand.GetAxis().y;
        }
    }

    void setTransformBtnClicked()
    {
        transformBtnClicked = true;
    }

    void moveOnAxis(int currentTransformableAxisInt, Vector3 rotAxis, Vector3 posAxis)
    {
        float speed = 5 * Time.deltaTime;
        currentTransformableAxis = (transformableAxis)currentTransformableAxisInt;
        Invoke("setTransformBtnClicked", 1);

        if (isRotation)
        {
            Zed_Greenscreen.Rotate(rotAxis * speed);
        }
        else
        {
            Zed_Greenscreen.localPosition += posAxis * speed / 10;            
        }
    }
}
