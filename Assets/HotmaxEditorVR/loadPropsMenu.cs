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
    [SerializeField]
    public GameObject propTilesContainer;

    private List<List<GameObject>> chunkedList = new List<List<GameObject>>();

    private int currentSpawnPage;
    private List<GameObject> spawnPages = new List<GameObject>();

    void Start()
    {
        _stateManagerMutatorRef = GameObject.FindObjectOfType(typeof(stateManager)) as stateManager;

        //create "chunks" groups of 4 to display the object
        chunkedList = ListExtensions.ChunkBy(propController.propObjects, 4);
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
        for (int i = 0; i < propTilesContainer.transform.childCount; i++)
        {
           Destroy(propTilesContainer.transform.GetChild(i).gameObject);
        }

        //load and arrange "chunks", 4 objects from the propObject List 
        for (int i = 0; i < chunkedList[page].Count; i++)
        {
            GameObject propTile = Instantiate(Resources.Load("propTile", typeof(GameObject))) as GameObject;

            //propTile class constructor
            propTile.GetComponent<propTile>().spawnableProp = chunkedList[page][i];
            propTile.GetComponent<propTile>().propTileText.text = chunkedList[page][i].name;

            propTile.transform.parent = propTilesContainer.transform;
            propTile.name = chunkedList[page][i].name + " tile";
            propTile.transform.localScale = new Vector3(2, 2, 2);
            propTile.transform.rotation = propTilesContainer.transform.rotation;
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