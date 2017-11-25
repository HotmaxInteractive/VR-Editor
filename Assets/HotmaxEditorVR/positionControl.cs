using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionControl : MonoBehaviour
{
    public objectSelect objSelect;
    editStateController stateController;

    float initialPadYPosition;
    float currentPadYPos;

    float distToController;
    float scrollSpeed = .2f;
    public float scrollDistance = 0;

    private bool _selectedObjectIsActive = stateManager.selectedObjectIsActive;

    private void Awake()
    {
        stateManager.selectedObjectIsActiveEvent += updateSelectedObjectIsActive;
    }

    protected virtual void OnApplicationQuit()
    {
        stateManager.selectedObjectIsActiveEvent -= updateSelectedObjectIsActive;
    }

    void updateSelectedObjectIsActive(bool value)
    {
        _selectedObjectIsActive = value;
        print(value);
    }

    private void OnEnable()
    {
        initialPadYPosition = objSelect.inputManagerRef.scrollY;
        distToController = Vector3.Distance(transform.position, objSelect.hand2.transform.position);
        scrollDistance = 0;
    }


    void Update()
    {
        if (_selectedObjectIsActive == true)
        {

            currentPadYPos = objSelect.inputManagerRef.scrollY;

            
            Vector3 offset = objSelect.hand2.transform.forward * (distToController + scrollDistance);
            transform.position = objSelect.hand2.transform.position + offset;

            if (currentPadYPos > initialPadYPosition)
            {
                scrollDistance += scrollSpeed;
                initialPadYPosition = currentPadYPos;
            }
            if (currentPadYPos < initialPadYPosition)
            {
                scrollDistance -= scrollSpeed;
                initialPadYPosition = currentPadYPos;
            }
        }
    }
}
