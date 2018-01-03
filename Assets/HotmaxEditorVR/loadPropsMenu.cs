using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class loadPropsMenu : MonoBehaviour
{
    //--local refs
    private stateManager _stateManagerMutatorRef;
    private stateManager.editorModes _editorMode = stateManager.editorMode;

    //--ref to props scene object
    private GameObject propParent;

    //--menu prop tile parent and child
    public GameObject propTiles;

    //--Lists to handle getting unique props in menu
    private List<string> propName = new List<string>();
    private List<GameObject> propObject = new List<GameObject>();
    private List<List<GameObject>> chunkedList = new List<List<GameObject>>();

    //--if false, disabled initial props on stage
    public bool propIsInStageAlready = false;

    private int currentPage;

    //TODO: for some reason the steamVR trigger event listener won't attach in enable or Awake. works in Start...
    private void OnEnable()
    {
        Invoke("waitToLoadTriggerEvent", .001f);
        stateManager.editorModeEvent += updateEditorMode;
    }

    void waitToLoadTriggerEvent()
    {
        inputManager.trackedController2.TriggerClicked += triggerClicked;
    }


    private void OnDisable()
    {
        inputManager.trackedController2.TriggerClicked -= triggerClicked;

        stateManager.editorModeEvent -= updateEditorMode;
    }

    void updateEditorMode(stateManager.editorModes value)
    {
        _editorMode = value;

        if(_editorMode == stateManager.editorModes.spawnMenuMode)
        {
            toggleMenuComponents(true);

            Vector3 offset = init.vrCamera.transform.forward * 2;
            transform.parent.transform.position = init.vrCamera.transform.position + offset;
            transform.parent.transform.LookAt(init.vrCamera.transform);
        }
        else
        {
            toggleMenuComponents(false);
        }
    }

    void Start()
    {
        _stateManagerMutatorRef = GameObject.FindObjectOfType(typeof(stateManager)) as stateManager;

        propParent = GameObject.Find("props");

        //get the names of all props in props
        for (int i = 0; i < propParent.transform.childCount; i++)
        {
            propName.Add(propParent.transform.GetChild(i).gameObject.name);
        }

        //weed out props that have the same name
        propName = propName.Distinct().ToList();

        //copy the game objects with the unique names to a gameObject list 
        for (int i = 0; i < propName.Count; i++)
        {
            propObject.Add(GameObject.Find(propName[i]));
        }

        //set all game objects in props inactive if they are not in the stage
        if (!propIsInStageAlready)
        {
            for (int i = 0; i < propParent.transform.childCount; i++)
            {
                propParent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        //create "chunks" groups of 4 to display the object
        chunkedList = ListExtensions.ChunkBy(propObject, 4);
        showPropObjectPage(0);
    }

    void showPropObjectPage(int page)
    {
        currentPage = page;
        //remove old tiles before loading new ones
        for (int i = 0; i < propTiles.transform.childCount; i++)
        {
            Destroy(propTiles.transform.GetChild(i).gameObject);
        }

        //load and arrange "chunks", 4 objects from the propObject List 
        for (int i = 0; i < chunkedList[page].Count; i++)
        {
            GameObject propTile = Instantiate(Resources.Load("propTile", typeof(GameObject))) as GameObject;
            propTile.transform.parent = propTiles.transform;
            propTile.transform.Find("propNameText").GetComponent<TextMesh>().text = chunkedList[page][i].name;
            propTile.name = chunkedList[page][i].name + " tile";
            propTile.transform.localScale = new Vector3(2, 2, 2);
            propTile.transform.rotation = propTiles.transform.rotation;
            switch (i)
            {
                case 0:
                    propTile.transform.localPosition = new Vector3(0, 0, 0);
                    displayPropInMenu(propTile, chunkedList[page][i], page, i);
                    break;
                case 1:
                    propTile.transform.localPosition = new Vector3(2.1f, 0, 0);
                    displayPropInMenu(propTile, chunkedList[page][i], page, i);
                    break;
                case 2:
                    propTile.transform.localPosition = new Vector3(0, -2.1f, 0);
                    displayPropInMenu(propTile, chunkedList[page][i], page, i);
                    break;
                case 3:
                    propTile.transform.localPosition = new Vector3(2.1f, -2.1f, 0);
                    displayPropInMenu(propTile, chunkedList[page][i], page, i);
                    break;
            }
        }
    }

    //show the prop model on the tile and rotate it around
    void displayPropInMenu(GameObject propTile, GameObject menuProp, int page, int i)
    {
        float biggestPropSide;

        GameObject menuPropDisplay = Instantiate(menuProp);
        menuPropDisplay.name = menuProp + " (visual)";

        //create a bounding box so the prop can be scaled proportionally
        GameObject boundingScaleBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        boundingScaleBox.GetComponent<MeshRenderer>().enabled = false;
        boundingScaleBox.GetComponent<BoxCollider>().enabled = false;
        boundingScaleBox.transform.position = propTile.transform.position;

        Vector3 menuPropDimension = Vector3.zero;

        //if the model has children, calculate the total bounds of all the children
        if (menuProp.transform.childCount > 0)
        {
            Bounds bounds = new Bounds(menuPropDisplay.transform.position, Vector3.zero);
            foreach (Renderer renderer in menuProp.GetComponentsInChildren<Renderer>())
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

        if (menuPropDimension.y >= menuPropDimension.x && menuPropDimension.y >= menuPropDimension.z)
        {
            biggestPropSide = menuPropDimension.y;
            boundingScaleBox.transform.localScale = new Vector3(biggestPropSide, biggestPropSide, biggestPropSide);
        }

        if (menuPropDimension.z >= menuPropDimension.x && menuPropDimension.z >= menuPropDimension.y)
        {
            biggestPropSide = menuPropDimension.z;
            boundingScaleBox.transform.localScale = new Vector3(biggestPropSide, biggestPropSide, biggestPropSide);
        }

        menuPropDisplay.transform.parent = boundingScaleBox.transform;
        menuPropDisplay.transform.localPosition = new Vector3(0, 0, 0);

        boundingScaleBox.transform.parent = propTile.transform;
        boundingScaleBox.transform.localScale = new Vector3(.6f, .6f, .6f);

        if (menuPropDisplay.GetComponent<Collider>())
        {
            menuPropDisplay.GetComponent<Collider>().enabled = false;
        }

        menuPropDisplay.AddComponent<slowRotation>();
        menuPropDisplay.SetActive(true);
    }

    void triggerClicked(object sender, ClickedEventArgs e)
    {
        Ray ray = new Ray(inputManager.hand2.gameObject.transform.position, inputManager.hand2.gameObject.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {

            if (hit.collider.transform.parent == propTiles.transform)
            {
                //get the correct object from the current chunk
                GameObject newProp = Instantiate(chunkedList[currentPage][hit.transform.GetSiblingIndex()]);
                newProp.name = chunkedList[currentPage][hit.transform.GetSiblingIndex()].name;
                newProp.transform.parent = propParent.transform;
                newProp.SetActive(true);

                //set the spawned object to the new selected object
                _stateManagerMutatorRef.SET_SELECTED_OBJECT(newProp);
                newProp.transform.position = hit.point;
                //check for prop -some objects are loaded with props, some aren't. 
                if(!newProp.GetComponent<prop>())
                {
                    newProp.AddComponent<prop>();
                }
                newProp.AddComponent<activeProp>();
                _stateManagerMutatorRef.SET_SELECTED_OBJECT_IS_ACTIVE(true);

            }

            //page incrementer and decrementer
            if (hit.collider.gameObject.name == "pageUp")
            {
                if (currentPage >= chunkedList.Count - 1)
                {
                    return;
                }
                else
                {
                    currentPage += 1;
                    showPropObjectPage(currentPage);
                }
            }
            if (hit.collider.gameObject.name == "pageDown")
            {
                if (currentPage <= 0)
                {
                    return;
                }
                else
                {
                    currentPage -= 1;
                    showPropObjectPage(currentPage);
                }
            }
            //removes the menu
            if (hit.collider.gameObject.name == "closeMenu")
            {
                toggleMenuComponents(false);
            }
        }
    }

    void toggleMenuComponents(bool on)
    {
        foreach (Transform child in transform.parent.GetChild(0))
        {
            child.gameObject.SetActive(on);
        }
        foreach (Transform child in transform.parent.GetChild(1))
        {
            child.gameObject.SetActive(on);
        }
    }
}