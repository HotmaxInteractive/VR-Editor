using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propTile : MonoBehaviour, IHittable
{
    //--local refs
    private stateManager _stateManagerMutatorRef;

    //--Note: FAUX CONSTRUCTOR - properties passed on prefab instantiation
    public GameObject spawnableProp;
    public TextMesh propTileText;

    private GameObject boundingScaleBox;

    void Start()
    {
        _stateManagerMutatorRef = init._stateManagerMutatorRef;
        //displayPropInMenu(spawnableProp);
    }

    public void receiveHit(RaycastHit hit)
    {
        //get the correct object from the current chunk
        GameObject newProp = Instantiate(spawnableProp);
        newProp.name = spawnableProp.name;
        newProp.transform.parent = init.props.transform;
        newProp.SetActive(true);

        //set the spawned object to the new selected object
        _stateManagerMutatorRef.SET_SELECTED_OBJECT(newProp);
        newProp.transform.position = hit.point;

        newProp.AddComponent<activeProp>();
        _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(true);
    }

    //show the prop model on the tile and rotate it around
    void displayPropInMenu(GameObject menuProp)
    {
        float biggestPropSide;

        GameObject menuPropDisplay = Instantiate(menuProp);
        Vector3 menuPropDimension = Vector3.zero;

        menuPropDisplay.name = menuProp.name + " (visual)";

        //create a bounding box so the prop can be scaled proportionally
        boundingScaleBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        boundingScaleBox.GetComponent<MeshRenderer>().enabled = false;
        boundingScaleBox.GetComponent<BoxCollider>().enabled = false;
        boundingScaleBox.transform.position = transform.position;

        //if the model has children, calculate the total bounds of all the children
        if (menuPropDisplay.transform.childCount > 0)
        {
            Bounds bounds = new Bounds(menuPropDisplay.transform.position, Vector3.zero);
            foreach (Renderer renderer in menuPropDisplay.GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(renderer.bounds);
            }
            menuPropDimension = bounds.size;
        }
        //only calculate the bounds of the prop
        else
        {
            menuPropDimension = menuPropDisplay.GetComponent<Renderer>().bounds.size;
        }

        //compare the sides of the menu prop to find the largest side
        //set the bounding scale box to fit around the menu prop
        if (menuPropDimension.x >= menuPropDimension.y && menuPropDimension.x >= menuPropDimension.z)
        {
            biggestPropSide = menuPropDimension.x;
            boundingScaleBox.transform.localScale = new Vector3(biggestPropSide, biggestPropSide, biggestPropSide);
        }
        else if (menuPropDimension.y >= menuPropDimension.x && menuPropDimension.y >= menuPropDimension.z)
        {
            biggestPropSide = menuPropDimension.y;
            boundingScaleBox.transform.localScale = new Vector3(biggestPropSide, biggestPropSide, biggestPropSide);
        }
        else
        {
            biggestPropSide = menuPropDimension.z;
            boundingScaleBox.transform.localScale = new Vector3(biggestPropSide, biggestPropSide, biggestPropSide);
        }

        menuPropDisplay.transform.parent = boundingScaleBox.transform;
        menuPropDisplay.transform.localPosition = new Vector3(0, 0, 0);

        boundingScaleBox.transform.parent = transform;
        boundingScaleBox.transform.localScale = new Vector3(.6f, .6f, .6f);

        if (menuPropDisplay.GetComponent<Collider>())
        {
            menuPropDisplay.GetComponent<Collider>().enabled = false;
        }

        menuPropDisplay.SetActive(true);
    }

    private void Update()
    {
        //boundingScaleBox.transform.Rotate(0, 1 * Time.deltaTime * 30, 0);
    }
}
