using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class propertiesMenu : MonoBehaviour
{
    //-----DEPENDENCIES-----\\
    //--All materials must be in a Materials folder in the "Resources"
    //--prefab called "lightSource" must be in the Resources folder
    //--All imported textures must be read/writable

    //--local refs
    private GameObject _selectedObject = stateManager.selectedObject;
    private stateManager.editorModes _editorMode = stateManager.editorMode;
    private Vector3 _raycastHitPoint;
    private GameObject _raycastHitObject;


    //--add all materials in object to list
    private List<string> materialNames = new List<string>();
    private List<List<string>> chunkedMaterialList = new List<List<string>>();
    private int currentMaterialsPage = 0;

    //--refs to children objects
    public GameObject materialListHolder;
    public GameObject slider;
    public TextMesh physicsText;

    private Rigidbody selectedObjectRb;

    //--slider stuff
    private bool sliderMoving = false;
    float sliderUnit;

    private bool massScaleHit = false;

    void Start()
    {
        stateManager.selectedObjectEvent += updateSelectedObject;
        stateManager.raycastHitInfoEvent += updateRaycastHitInfoEvent;
    }

    private void OnApplicationQuit()
    {
        stateManager.selectedObjectEvent -= updateSelectedObject;
        stateManager.raycastHitInfoEvent -= updateRaycastHitInfoEvent;
    }

    void updateSelectedObject(GameObject value)
    {
        _selectedObject = value;

        if (_selectedObject != null)
        {
            currentMaterialsPage = 0;

            //--------------------------------------MATERIALS--------------------------------------\\
            //before we add new materials clear out the list
            materialNames.Clear();

            //if the hit object has a renderer, get the material
            if (_selectedObject.GetComponent<Renderer>())
            {
                //if there is more than one material loop and add
                for (int i = 0; i < _selectedObject.GetComponent<Renderer>().materials.Length; i++)
                {
                    materialNames.Add(_selectedObject.GetComponent<Renderer>().materials[i].name);
                }
            }

            //get all the materials in the children
            //--TODO: Do this on GLTF serialization
            Renderer[] renderers = _selectedObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                materialNames.Add(renderer.material.name);
            }

            removeLastMaterialPage();

            //get only unique instances of the material names
            materialNames = materialNames.Distinct().ToList();

            //create "chunks" groups of 4 to display the object
            chunkedMaterialList = ListExtensions.ChunkBy(materialNames, 4);
            showMaterialPage(0);

            //--------------------------------------MASS--------------------------------------\\

            //--GETTING
            //if it has a Rb it should be under 90 mass units
            if (_selectedObject.GetComponent<Rigidbody>())
            {
                selectedObjectRb = _selectedObject.GetComponent<Rigidbody>();

                if (selectedObjectRb.mass < 60)
                {
                    physicsText.text = "#grabbable #throwable";
                }
                else if (selectedObjectRb.mass > 60 && selectedObjectRb.mass < 90)
                {
                    physicsText.text = "#heavy #non-grabbable";
                }

                slider.transform.localPosition = new Vector3(0, 0, selectedObjectRb.mass / 10);
            }
            else
            {
                slider.transform.localPosition = new Vector3(0, 0, 10);
                physicsText.text = "#non-moveable";
            }
        }
    }

    public void materialPageUp()
    {
        if (currentMaterialsPage >= chunkedMaterialList.Count - 1)
        {
            return;
        }
        else
        {
            currentMaterialsPage += 1;
            removeLastMaterialPage();
            showMaterialPage(currentMaterialsPage);
        }
    }

    public void materialPageDown()
    {
        if (currentMaterialsPage <= 0)
        {
            return;
        }
        else
        {
            currentMaterialsPage -= 1;
            removeLastMaterialPage();
            showMaterialPage(currentMaterialsPage);
        }
    }

    void updateRaycastHitInfoEvent(Vector3 value1, GameObject value2)
    {
        _raycastHitPoint = value1;
        _raycastHitObject = value2;
    }

    public void updatePhysicsInfo()
    {
        //TODO: move this to a slider class mutate, these properties and animations will probably have to go through the state

        //--TODO: needs to be a public method for the trigger unclick of the slider
        if(_selectedObject != null)
        {
            sliderUnit = slider.transform.localPosition.z;

            if (sliderUnit < 6)
            {
                if (!_selectedObject.GetComponent<Rigidbody>())
                {
                    _selectedObject.AddComponent<Rigidbody>();
                }
                _selectedObject.GetComponent<Rigidbody>().mass = sliderUnit * 10;
                _selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                physicsText.text = "#grabbable #throwable";

            }
            else if (sliderUnit > 6 && sliderUnit < 9)
            {
                if (!_selectedObject.GetComponent<Rigidbody>())
                {
                    _selectedObject.AddComponent<Rigidbody>();
                    selectedObjectRb = _selectedObject.GetComponent<Rigidbody>();
                }
                _selectedObject.GetComponent<Rigidbody>().mass = sliderUnit * 10;
                _selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                physicsText.text = "#heavy #non-grabbable";
            }
            else
            {
                if (_selectedObject.GetComponent<Rigidbody>())
                {
                    Destroy(_selectedObject.GetComponent<Rigidbody>());
                }
                physicsText.text = "#non-moveable";
            }
            sliderMoving = false;
            
        }
    }

    void showMaterialPage(int page)
    {
        //put the unique materials on spheres to visualize in the menu
        for (int i = 0; i < chunkedMaterialList[page].Count; i++)
        {
            GameObject materialSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            materialSphere.GetComponent<Collider>().isTrigger = true;
            materialSphere.transform.parent = materialListHolder.transform;
            materialSphere.transform.localScale = new Vector3(1, 1, 1);
            materialSphere.transform.localPosition = new Vector3(i, 0, 0);
            materialSphere.GetComponent<Renderer>().material = Resources.Load("Materials/" + chunkedMaterialList[page][i].Replace(" (Instance)", ""), typeof(Material)) as Material;
            materialSphere.name = materialSphere.GetComponent<Renderer>().material.name.Replace(" (Instance)", "");
        }
    }

    //clear out the material spheres
    void removeLastMaterialPage()
    {
        for (int i = 0; i < materialListHolder.transform.childCount; i++)
        {
            Destroy(materialListHolder.transform.GetChild(i).gameObject);
        }
    }
}
