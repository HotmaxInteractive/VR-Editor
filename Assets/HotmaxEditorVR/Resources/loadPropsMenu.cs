using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class loadPropsMenu : MonoBehaviour
{
    //--local refs
    private stateManager _stateManagerMutatorRef;

    //--ref to props scene object
    private GameObject propParent;

    //--menu prop tile parent and child
    private GameObject propTiles;
    private GameObject menuProp;

    //--Lists to handle getting unique props in menu
    private List<string> propName = new List<string>();
    private List<GameObject> propObject = new List<GameObject>();
    private List<List<GameObject>> chunkedList = new List<List<GameObject>>();

    //--check if props are in the scene already
    public bool propIsInStageAlready = false;

    private int currentPage;

    private stateManager.editorModes _editorMode = stateManager.editorMode;


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

        if(_editorMode == stateManager.editorModes.openMenuMode)
        {
            toggleMenuComponents(true);
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
        propTiles = GameObject.Find("propTiles");

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
        GameObject menuPropDisplay = Instantiate(menuProp);
        menuPropDisplay.name = menuProp + " (visual)";
        menuPropDisplay.transform.parent = propTile.transform;
        menuPropDisplay.transform.rotation = propTile.transform.rotation;
        menuPropDisplay.transform.localPosition = new Vector3(0, 0, 0);
        menuPropDisplay.transform.localScale = menuPropDisplay.transform.localScale / 20;

        //TODO: right now this scales the model down reletive to the bounding box of the propTile. it doesn't work for oblong objects
        menuPropDisplay.transform.localScale = (propTile.GetComponent<Renderer>().bounds.size.magnitude) * menuPropDisplay.transform.localScale;

        //TODO: there is probably a better way to do this
        if (menuPropDisplay.GetComponent<Rigidbody>())
        {
            menuPropDisplay.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (menuPropDisplay.GetComponent<Collider>())
        {
            Destroy(menuPropDisplay.GetComponent<Collider>());
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
                newProp.SetActive(true);

                //set the spawned object to the new selected object
                _stateManagerMutatorRef.SET_SELECTED_OBJECT(newProp);
                newProp.transform.position = hit.point;
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
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(on);
        }
    }
}


//a handy helper method that turns a list into a chunked list
public static class ListExtensions
{
    public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }
}
