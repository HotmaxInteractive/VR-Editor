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

    //--add all materials in object to list
    private List<string> materialNames = new List<string>();
    private List<List<string>> chunkedMaterialList = new List<List<string>>();
    private int currentPage = 0;

    //--refs to children objects
    private GameObject materialListHolder;
    private GameObject slider;
    private TextMesh physicsText;
    private TextMesh animatorText;

    private Rigidbody selectedObjectRb;

    //--slider stuff
    private bool sliderMoving = false;
    float sliderUnit;

    private bool hasLocalAnimator = false;

    private bool triggerDown = false;

    void Start()
    {
        stateManager.selectedObjectEvent += updateSelectedObject;
        stateManager.editorModeEvent += updateEditorMode;

        inputManager.trackedController2.TriggerClicked += triggerClicked;
        inputManager.trackedController2.TriggerUnclicked += triggerUnclicked;

        //--menu children
        materialListHolder = GameObject.Find("materialListHolder");
        slider = GameObject.Find("massSlider");
        physicsText = GameObject.Find("physicsText").GetComponent<TextMesh>();
        animatorText = GameObject.Find("animatorText").GetComponent<TextMesh>();
    }

    private void OnApplicationQuit()
    {
        stateManager.selectedObjectEvent -= updateSelectedObject;
        stateManager.editorModeEvent -= updateEditorMode;

        inputManager.trackedController2.TriggerClicked -= triggerClicked;
        inputManager.trackedController2.TriggerUnclicked -= triggerUnclicked;
    }

    void updateSelectedObject(GameObject value)
    {
        _selectedObject = value;

        if (_selectedObject.GetComponent<MonoBehaviour>() is prop)
        {
            currentPage = 0;

            //TODO: check to see if the animation class is attached
            animatorText.text = "#no animation";

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

    void updateEditorMode(stateManager.editorModes value)
    {
        _editorMode = value;

        if (_editorMode == stateManager.editorModes.propertiesMenuMode)
        {
            toggleMenuComponents(true);
        }
        else
        {
            toggleMenuComponents(false);
        }
    }

    //--Handles the menu buttons
    void triggerClicked(object sender, ClickedEventArgs e)
    {
        triggerDown = true;

        RaycastHit hit;
        Ray ray = new Ray(inputManager.hand2.gameObject.transform.position, inputManager.hand2.gameObject.transform.forward);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.name == "materialPageUp")
            {
                if (currentPage >= chunkedMaterialList.Count - 1)
                {
                    return;
                }
                else
                {
                    currentPage += 1;
                    removeLastMaterialPage();
                    showMaterialPage(currentPage);
                }
            }

            if (hit.collider.gameObject.name == "materialPageDown")
            {
                if (currentPage <= 0)
                {
                    return;
                }
                else
                {
                    currentPage -= 1;
                    removeLastMaterialPage();
                    showMaterialPage(currentPage);
                }
            }

            if (hit.collider.gameObject.name == "closePropertiesMenu")
            {
                toggleMenuComponents(false);
            }

            //--SETTING (just updating the UI for now)
            if (hit.collider.gameObject.name == "toggleHasLocalAnimator")
            {
                if (hasLocalAnimator)
                {
                    //TODO: remove animator
                    animatorText.text = "#no-animation";
                    hasLocalAnimator = false;
                }
                else
                {
                    //TODO: Add animation controller class
                    animatorText.text = "#has-animation";
                    hasLocalAnimator = true;
                }
            }           
        }
    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        triggerDown = false;

        MeshCollider selectedObjectMeshCollider = _selectedObject.GetComponent<MeshCollider>();

        if (sliderMoving)
        {
            if (sliderUnit < 6)
            {
                if (!_selectedObject.GetComponent<Rigidbody>())
                {
                    if (selectedObjectMeshCollider && selectedObjectMeshCollider.convex == false)
                    {
                        selectedObjectMeshCollider.convex = true;
                    }
                    _selectedObject.AddComponent<Rigidbody>();
                }
                _selectedObject.GetComponent<Rigidbody>().mass = sliderUnit * 10;
                physicsText.text = "#grabbable #throwable";

            }
            else if (sliderUnit > 6 && sliderUnit < 9)
            {
                if (!_selectedObject.GetComponent<Rigidbody>())
                {
                    if (selectedObjectMeshCollider && selectedObjectMeshCollider.convex == false)
                    {
                        selectedObjectMeshCollider.convex = true;
                    }
                    _selectedObject.AddComponent<Rigidbody>();
                    selectedObjectRb = _selectedObject.GetComponent<Rigidbody>();
                }
                _selectedObject.GetComponent<Rigidbody>().mass = sliderUnit * 10;
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

    void Update()
    {
        if (triggerDown)
        {
            RaycastHit hit;
            Ray ray = new Ray(inputManager.hand2.gameObject.transform.position, inputManager.hand2.gameObject.transform.forward);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.gameObject == slider.transform.parent.transform.parent.gameObject)
                {
                    slider.transform.position = hit.point;
                    slider.transform.localPosition = new Vector3(0, 0, slider.transform.localPosition.z);
                    sliderUnit = slider.transform.localPosition.z;
                    sliderMoving = true;
                }
            }
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

    void toggleMenuComponents(bool on)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(on);
        }
    }
}
