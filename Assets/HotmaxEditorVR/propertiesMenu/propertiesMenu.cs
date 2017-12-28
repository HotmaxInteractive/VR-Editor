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

    private List<string> materialNames = new List<string>();
    private List<List<string>> chunkedMaterialList = new List<List<string>>();
    private int currentPage = 0;

    private GameObject hitObject;
    private GameObject props;

    //--Game Objects that are in the the properties menu
    private GameObject materialListHolder;
    private GameObject slider;
    private TextMesh physicsText;
    private TextMesh animatorText;

    private GameObject selectedObject;
    private Rigidbody selectedObjectRb;

    private bool sliderMoving = false;
    float sliderUnit;

    private bool hasLocalAnimator = false;

    private stateManager.editorModes _editorMode = stateManager.editorMode;

    private bool triggerDown = false;

    void Start()
    {
        stateManager.editorModeEvent += updateEditorMode;

        inputManager.trackedController2.TriggerClicked += triggerClicked;
        inputManager.trackedController2.TriggerUnclicked += triggerUnclicked;

        materialListHolder = GameObject.Find("materialListHolder");
        slider = GameObject.Find("massSlider");

        physicsText = GameObject.Find("physicsText").GetComponent<TextMesh>();
        animatorText = GameObject.Find("animatorText").GetComponent<TextMesh>();

        props = GameObject.Find("props");
        selectedObject = props.transform.GetChild(0).gameObject;
    }

    private void OnApplicationQuit()
    {
        stateManager.editorModeEvent -= updateEditorMode;

        inputManager.trackedController2.TriggerClicked -= triggerClicked;
        inputManager.trackedController2.TriggerUnclicked -= triggerUnclicked;
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

            //--SETTING
            if (hit.collider.gameObject.name == "toggleHasLocalAnimator")
            {
                if (hasLocalAnimator)
                {
                    Destroy(hitObject.GetComponent<localAnimator>());
                    animatorText.text = "#no-animation";
                    hasLocalAnimator = false;
                }
                else
                {
                    hitObject.AddComponent<localAnimator>();
                    animatorText.text = "#has-animation";
                    hasLocalAnimator = true;
                }
            }

            if (hit.collider.gameObject.GetComponent<MonoBehaviour>() is prop)
            {
                print(hit.collider);
                //if their is a new hit object set the material page to 0
                GameObject lastHitObject = hitObject;
                if (lastHitObject != hit.collider.gameObject)
                {
                    currentPage = 0;
                }

                //--GETTING
                if (hit.collider.gameObject.GetComponent<localAnimator>())
                {
                    animatorText.text = "#has animation";
                    hasLocalAnimator = true;
                }
                else
                {
                    animatorText.text = "#no animation";
                    hasLocalAnimator = false;
                }

                //reset hitObject to the newly hit object
                hitObject = hit.collider.gameObject;
                selectedObject = hitObject;
                selectedObjectRb = selectedObject.GetComponent<Rigidbody>();

                //------------------------------------------MATERIALS------------------------------------------\\
                //before we add new materials clear out the list
                materialNames.Clear();

                //if the hit object has a renderer, get the material
                if (hitObject.GetComponent<Renderer>())
                {
                    //if there is more than one material loop and add
                    for (int i = 0; i < hitObject.GetComponent<Renderer>().materials.Length; i++)
                    {
                        materialNames.Add(hitObject.GetComponent<Renderer>().materials[i].name);
                    }
                }

                //get all the materials in the children
                Renderer[] renderers = hitObject.GetComponentsInChildren<Renderer>();
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
                if (selectedObject.GetComponent<Rigidbody>())
                {
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

    }

    void triggerUnclicked(object sender, ClickedEventArgs e)
    {
        triggerDown = false;

        if (sliderMoving)
        {
            if (sliderUnit < 6)
            {
                if (!selectedObject.GetComponent<Rigidbody>())
                {
                    if (selectedObject.GetComponent<MeshCollider>() && selectedObject.GetComponent<MeshCollider>().convex == false)
                    {
                        selectedObject.GetComponent<MeshCollider>().convex = true;
                    }
                    selectedObject.AddComponent<Rigidbody>();
                }
                selectedObject.GetComponent<Rigidbody>().mass = sliderUnit * 10;
                physicsText.text = "#grabbable #throwable";

            }
            else if (sliderUnit > 6 && sliderUnit < 9)
            {
                if (!selectedObject.GetComponent<Rigidbody>())
                {
                    if (selectedObject.GetComponent<MeshCollider>() && selectedObject.GetComponent<MeshCollider>().convex == false)
                    {
                        selectedObject.GetComponent<MeshCollider>().convex = true;
                    }
                    selectedObject.AddComponent<Rigidbody>();
                }
                selectedObject.GetComponent<Rigidbody>().mass = sliderUnit * 10;
                physicsText.text = "#heavy #non-grabbable";
            }
            else
            {
                if (selectedObject.GetComponent<Rigidbody>())
                {
                    Destroy(selectedObject.GetComponent<Rigidbody>());
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
