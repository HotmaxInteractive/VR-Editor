using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectSelect : MonoBehaviour
{
    public Valve.VR.InteractionSystem.Hand hand1;
    public Valve.VR.InteractionSystem.Hand hand2;

    public Vector3 endPosition;

    public LineRenderer laserLineRenderer;
    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;

    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.endWidth = laserWidth;
    }

    void Update()
    {
        if (Input.GetButtonDown("triggerRight"))
        {
            selected(hand2.gameObject.transform.position, hand2.gameObject.transform.forward);
        }

        ShootLaserFromTargetPosition(hand2.gameObject.transform.position, hand2.gameObject.transform.forward, laserMaxLength);
    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        endPosition = targetPosition + (length * direction);

        if (Physics.Raycast(ray, out raycastHit, length))
        {
            endPosition = raycastHit.point;
        }

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, endPosition);
    }

    void selected(Vector3 targetPosition, Vector3 direction)
    {
        RaycastHit hit;
        Ray ray = new Ray(targetPosition, direction);
        if (Physics.Raycast(ray, out hit))
        {
            //check to see if the raycast is hitting a game object
            if (hit.collider != null && !hit.collider.name.Contains("structure"))
            {
                GameObject hitObject = hit.collider.gameObject;
                //turn off collider in selected object, we don't need it for now
                hit.collider.enabled = false;

                //Add object controllers and reference this class
                hitObject.AddComponent<positionControl>();
                hitObject.GetComponent<positionControl>().objSelect = this;

                hitObject.AddComponent<rotationControl>();
                hitObject.GetComponent<rotationControl>().objSelect = this;

                hitObject.AddComponent<scaleControl>();
                hitObject.GetComponent<scaleControl>().objSelect = this;

                hitObject.AddComponent<editStateController>();            

                removeDecorators();              

                //Add the "selectable outline"
                hitObject.AddComponent<cakeslice.Outline>();
            }
        }
    }

    void removeDecorators()
    {
        //Clean up old highlighted object before adding new stuff
        cakeslice.Outline outline = (cakeslice.Outline)FindObjectOfType(typeof(cakeslice.Outline));
        if (outline)
        {
            GameObject highlightedObject = outline.transform.gameObject;
            highlightedObject.GetComponent<Collider>().enabled = true;
            Destroy(highlightedObject.GetComponent<positionControl>());
            Destroy(highlightedObject.GetComponent<rotationControl>());
            Destroy(highlightedObject.GetComponent<scaleControl>());
            Destroy(highlightedObject.GetComponent<editStateController>());
            Destroy(outline);
        }
    }
}
