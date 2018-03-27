using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class keyingTool : MonoBehaviour
{
    //--local refs
    private bool _arModeIsOn = stateManager.arModeIsOn;
    private stateManager _stateManagerMutatorRef;

    private string keyingDataFile = "keying_data.json";
    private string dataFilePath;

    private GreenScreenManager greenScreenManager;

    public GameObject greenScreenGradient;

    public Renderer currentKeyColor;

    public TextMeshPro rangeText;
    public TextMeshPro smoothingText;
    public TextMeshPro clipWhiteText;
    public TextMeshPro clipBlackText;
    public TextMeshPro erosionText;
    public TextMeshPro desplillText;

    public GameObject rangeSlider;
    public GameObject smoothingSlider;
    public GameObject clipWhiteSlider;
    public GameObject clipBlackSlider;
    public GameObject erosionSlider;
    public GameObject despillSlider;

    private void Start()
    {
        dataFilePath = Path.Combine(Application.streamingAssetsPath, keyingDataFile);

        stateManager.arModeIsOnEvent += updateARModeIsOn;
        greenScreenManager = FindObjectOfType<GreenScreenManager>();

        if (_arModeIsOn)
        {
            //set range and smoothness green screen vals to 0
            greenScreenManager.range = 0;
            greenScreenManager.smoothness = 0;
            greenScreenManager.UpdateShader();
        }
    }

    private void OnApplicationQuit()
    {
        stateManager.arModeIsOnEvent -= updateARModeIsOn;
    }

    private void OnEnable()
    {
        loadChromaKeyData();

        if (_arModeIsOn && greenScreenManager != null)
        {
            //set range and smoothness green screen vals to 0
            greenScreenManager.range = 0;
            greenScreenManager.smoothness = 0;
            greenScreenManager.UpdateShader();
        }
    }

    //--fires when keying tools toggled off as well as depthcast app shuts down
    private void OnDisable()
    {
        saveChromaKeyData();
    }

    void Update()
    {
        setKeyColor();
        setChromaKeyValues();
    }


    void updateARModeIsOn(bool value)
    {
        _arModeIsOn = value;
        if (_arModeIsOn)
        {
            //set range and smoothness green screen vals to 0
            greenScreenManager.range = 0;
            greenScreenManager.smoothness = 0;
            greenScreenManager.UpdateShader();
        }
    }

    void setKeyColor()
    {
        RaycastHit hit;
        Ray ray = new Ray(inputManager.hand2.transform.position, inputManager.hand2.transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if (inputManager.trackedController2.triggerPressed)
            {
                //--Gets the pixel color from the green screen gradient swatch next to the actor monitor, then applies that to the keyColors of "GreenScreenManager"
                if (hit.collider.gameObject == greenScreenGradient)
                {
                    Renderer rend = hit.transform.GetComponent<Renderer>();
                    MeshCollider meshCollider = hit.collider as MeshCollider;

                    if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                    {
                        return;
                    }

                    Texture2D tex = rend.material.mainTexture as Texture2D;
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= tex.width;
                    pixelUV.y *= tex.height;

                    //--Sets the keyColors to the selected pixel color
                    greenScreenManager.keyColors = tex.GetPixel((int)pixelUV.x, (int)pixelUV.y);
                    greenScreenManager.UpdateShader();
                    //--Sets the currentKeyColor to the new color too
                    currentKeyColor.material.color = greenScreenManager.keyColors;
                }
            }
        }
    }

    void setChromaKeyValues()
    {
        if (inputManager.trackedController2.triggerPressed)
        {
            greenScreenManager.range = rangeSlider.transform.localPosition.x;
            greenScreenManager.smoothness = smoothingSlider.transform.localPosition.x;
            greenScreenManager.whiteClip = clipWhiteSlider.transform.localPosition.x;
            greenScreenManager.blackClip = clipBlackSlider.transform.localPosition.x;
            greenScreenManager.erosion = Mathf.RoundToInt(erosionSlider.transform.localPosition.x * 5);
            greenScreenManager.spill = despillSlider.transform.localPosition.x;
            updateTextValues();
            greenScreenManager.UpdateShader();
        }
    }

    void updateSliderPositions()
    {
        rangeSlider.transform.localPosition = new Vector3(greenScreenManager.range, rangeSlider.transform.localPosition.y, rangeSlider.transform.localPosition.z);
        smoothingSlider.transform.localPosition = new Vector3(greenScreenManager.smoothness, smoothingSlider.transform.localPosition.y, smoothingSlider.transform.localPosition.z);
        clipWhiteSlider.transform.localPosition = new Vector3(greenScreenManager.whiteClip, clipWhiteSlider.transform.localPosition.y, clipWhiteSlider.transform.localPosition.z);
        clipBlackSlider.transform.localPosition = new Vector3(greenScreenManager.blackClip, clipBlackSlider.transform.localPosition.y, clipBlackSlider.transform.localPosition.z);
        erosionSlider.transform.localPosition = new Vector3(greenScreenManager.erosion / (float)5, erosionSlider.transform.localPosition.y, erosionSlider.transform.localPosition.z);
        despillSlider.transform.localPosition = new Vector3(greenScreenManager.spill, despillSlider.transform.localPosition.y, despillSlider.transform.localPosition.z);
    }

    void updateTextValues()
    {
        rangeText.text = greenScreenManager.range.ToString();
        smoothingText.text = greenScreenManager.smoothness.ToString();
        clipWhiteText.text = greenScreenManager.whiteClip.ToString();
        clipBlackText.text = greenScreenManager.blackClip.ToString();
        erosionText.text = greenScreenManager.erosion.ToString();
        desplillText.text = greenScreenManager.spill.ToString();
    }

    public void saveChromaKeyData()
    {
        saveChromakeyValues saveData = new saveChromakeyValues();
        saveData.keyColor = greenScreenManager.keyColors;
        saveData.range = greenScreenManager.range;
        saveData.smoothness = greenScreenManager.smoothness;
        saveData.whiteClip = greenScreenManager.whiteClip;
        saveData.blackClip = greenScreenManager.blackClip;
        saveData.erosion = greenScreenManager.erosion;
        saveData.despill = greenScreenManager.spill;

        //Convert to Json
        string jsonData = JsonUtility.ToJson(saveData);
        //Save Json string
        File.WriteAllText(dataFilePath, jsonData);
    }

    public void loadChromaKeyData()
    {
        if (File.Exists(dataFilePath))
        {
            //Load saved Json
            string jsonData = File.ReadAllText(dataFilePath);
            //Convert to Class
            saveChromakeyValues loadedData = JsonUtility.FromJson<saveChromakeyValues>(jsonData);

            //Display saved data
            greenScreenManager.keyColors = loadedData.keyColor;
            greenScreenManager.range = loadedData.range;
            greenScreenManager.smoothness = loadedData.smoothness;
            greenScreenManager.whiteClip = loadedData.whiteClip;
            greenScreenManager.blackClip = loadedData.blackClip;
            greenScreenManager.erosion = loadedData.erosion;
            greenScreenManager.spill = loadedData.despill;

            currentKeyColor.material.color = greenScreenManager.keyColors;

            updateSliderPositions();
            updateTextValues();

            greenScreenManager.UpdateShader();
        }
    }
}

[SerializeField]
public class saveChromakeyValues
{
    public Color keyColor = Color.green;
    public float range = 0;
    public float smoothness = 0;
    public float whiteClip = 0;
    public float blackClip = 0;
    public int erosion = 0;
    public float despill = 0;
}

