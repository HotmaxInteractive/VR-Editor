using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class loadPropsMenu : MonoBehaviour
{
    //--local refs
    private stateManager _stateManagerMutatorRef;
    private stateManager.editorModes _editorMode = stateManager.editorMode;
    private bool _playerIsLocomoting = stateManager.playerIsLocomoting;

    //--child gameObject refs
    public GameObject propTiles;
    
    //--Lists to handle getting unique props in menu
    private List<string> propName = new List<string>();
    private List<GameObject> propObject = new List<GameObject>();
    private List<List<GameObject>> chunkedList = new List<List<GameObject>>();

    //--if false, disabled initial props on stage
    //--TODO: remove when props are received from PropClass
    public bool propIsInStageAlready = false;

    private int currentSpawnPage;

    void Start()
    {
        _stateManagerMutatorRef = GameObject.FindObjectOfType(typeof(stateManager)) as stateManager;

        //get the names of all props in props
        //TODO: we will be getting the props from a propClass in the form of a List of Serialized GameObjects
        for (int i = 0; i < init.props.transform.childCount; i++)
        {
            propName.Add(init.props.transform.GetChild(i).gameObject.name);
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
            for (int i = 0; i < init.props.transform.childCount; i++)
            {
                init.props.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        //create "chunks" groups of 4 to display the object
        chunkedList = ListExtensions.ChunkBy(propObject, 4);
        showPropObjectPage(0);
    }

   public void spawnPageUp()
    {
        if (currentSpawnPage >= chunkedList.Count - 1)
        {
            return;
        }
        else
        {
            currentSpawnPage += 1;
            showPropObjectPage(currentSpawnPage);
        }
    }

    public void spawnPageDown()
    {
        if (currentSpawnPage <= 0)
        {
            return;
        }
        else
        {
            currentSpawnPage -= 1;
            showPropObjectPage(currentSpawnPage);
        }
    }

    void showPropObjectPage(int page)
    {
        currentSpawnPage = page;
        //remove old tiles before loading new ones
        for (int i = 0; i < propTiles.transform.childCount; i++)
        {
            Destroy(propTiles.transform.GetChild(i).gameObject);
        }

        //load and arrange "chunks", 4 objects from the propObject List 
        for (int i = 0; i < chunkedList[page].Count; i++)
        {
            GameObject propTile = Instantiate(Resources.Load("propTile", typeof(GameObject))) as GameObject;

            //propTile class constructor
            propTile.GetComponent<propTile>().spawnableProp = chunkedList[page][i];
            propTile.GetComponent<propTile>().currentPage = page;
            propTile.GetComponent<propTile>().propTileText.text = chunkedList[page][i].name;


            propTile.transform.parent = propTiles.transform;
            propTile.name = chunkedList[page][i].name + " tile";
            propTile.transform.localScale = new Vector3(2, 2, 2);
            propTile.transform.rotation = propTiles.transform.rotation;
            switch (i)
            {
                case 0:
                    propTile.transform.localPosition = new Vector3(0, 0, 0);
                    break;
                case 1:
                    propTile.transform.localPosition = new Vector3(2.1f, 0, 0);
                    break;
                case 2:
                    propTile.transform.localPosition = new Vector3(0, -2.1f, 0);
                    break;
                case 3:
                    propTile.transform.localPosition = new Vector3(2.1f, -2.1f, 0);
                    break;
            }
        }
    }
}