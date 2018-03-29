using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class newKeyingTool : MonoBehaviour
{
    //--local refs
    private bool _arModeIsOn = stateManager.arModeIsOn;
    private stateManager _stateManagerMutatorRef;

    private string keyingDataFile = "keying_data.json";
    private string keyingDataFilePath;

    private GreenScreenManager greenScreenManager;

    public GameObject greenScreenGradient;

    public Renderer currentKeyColor;

    public TextMeshPro rangeText;
    public TextMeshPro smoothingText;
    public TextMeshPro clipWhiteText;
    public TextMeshPro clipBlackText;
    public TextMeshPro erosionText;
    public TextMeshPro desplillText;
    public TextMeshPro exposureText;
    public TextMeshPro gainText;

    public GameObject rangeSlider;
    public GameObject smoothingSlider;
    public GameObject clipWhiteSlider;
    public GameObject clipBlackSlider;
    public GameObject erosionSlider;
    public GameObject despillSlider;
    public GameObject exposureSlider;
    public GameObject gainSlider;

    private void Start()
    {
        greenScreenManager = FindObjectOfType<GreenScreenManager>();
        if (greenScreenManager != null)
        {
            loadChromaKeyData();
        }
    }

    void Update()
    {
        setKeyColor();
        setChromaKeyValues();
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

            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.EXPOSURE, Mathf.RoundToInt(exposureSlider.transform.localPosition.x * 100));
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.GAIN, Mathf.RoundToInt(gainSlider.transform.localPosition.x * 100));

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

        exposureSlider.transform.localPosition = new Vector3(sl.ZEDCamera.GetInstance().GetCameraSettings(sl.CAMERA_SETTINGS.EXPOSURE) / (float)100, exposureSlider.transform.localPosition.y, exposureSlider.transform.localPosition.z);
        gainSlider.transform.localPosition = new Vector3(sl.ZEDCamera.GetInstance().GetCameraSettings(sl.CAMERA_SETTINGS.GAIN) / (float)100, gainSlider.transform.localPosition.y, gainSlider.transform.localPosition.z);
    }

    void updateTextValues()
    {
        rangeText.text = greenScreenManager.range.ToString();
        smoothingText.text = greenScreenManager.smoothness.ToString();
        clipWhiteText.text = greenScreenManager.whiteClip.ToString();
        clipBlackText.text = greenScreenManager.blackClip.ToString();
        erosionText.text = greenScreenManager.erosion.ToString();
        desplillText.text = greenScreenManager.spill.ToString();

        exposureText.text = sl.ZEDCamera.GetInstance().GetCameraSettings(sl.CAMERA_SETTINGS.EXPOSURE).ToString();
        gainText.text = sl.ZEDCamera.GetInstance().GetCameraSettings(sl.CAMERA_SETTINGS.GAIN).ToString();
    }

    public void saveChromaKeyData()
    {
        saveChromakeyData saveData = new saveChromakeyData();
        saveData.keyColor = greenScreenManager.keyColors;
        saveData.range = greenScreenManager.range;
        saveData.smoothness = greenScreenManager.smoothness;
        saveData.whiteClip = greenScreenManager.whiteClip;
        saveData.blackClip = greenScreenManager.blackClip;
        saveData.erosion = greenScreenManager.erosion;
        saveData.despill = greenScreenManager.spill;

        saveData.exposure = sl.ZEDCamera.GetInstance().GetCameraSettings(sl.CAMERA_SETTINGS.EXPOSURE);
        saveData.gain = sl.ZEDCamera.GetInstance().GetCameraSettings(sl.CAMERA_SETTINGS.GAIN);

        //Convert to Json
        string jsonData = JsonUtility.ToJson(saveData);
        //Save Json string
        File.WriteAllText(keyingDataFilePath, jsonData);
    }

    public void loadChromaKeyData()
    {
        keyingDataFilePath = Path.Combine(Application.streamingAssetsPath, keyingDataFile);
        if (File.Exists(keyingDataFilePath))
        {
            //Load saved Json
            string jsonData = File.ReadAllText(keyingDataFilePath);
            //Convert to Class
            saveChromakeyData loadedData = JsonUtility.FromJson<saveChromakeyData>(jsonData);

            //Display saved data
            greenScreenManager.keyColors = loadedData.keyColor;
            greenScreenManager.range = loadedData.range;
            greenScreenManager.smoothness = loadedData.smoothness;
            greenScreenManager.whiteClip = loadedData.whiteClip;
            greenScreenManager.blackClip = loadedData.blackClip;
            greenScreenManager.erosion = loadedData.erosion;
            greenScreenManager.spill = loadedData.despill;

            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.EXPOSURE, loadedData.exposure);
            sl.ZEDCamera.GetInstance().SetCameraSettings(sl.CAMERA_SETTINGS.GAIN, loadedData.gain);

            currentKeyColor.material.color = greenScreenManager.keyColors;

            updateSliderPositions();
            updateTextValues();

            greenScreenManager.UpdateShader();
        }
        else
        {
            writeDefaults();
            loadChromaKeyData();
        }
    }

    void writeDefaults()
    {
        saveChromakeyData saveData = new saveChromakeyData();
        saveData.keyColor = Color.green;
        saveData.range = 0.30f;
        saveData.smoothness = 0;
        saveData.whiteClip = 0;
        saveData.blackClip = 0;
        saveData.erosion = 0;
        saveData.despill = 0;

        saveData.exposure = 50;
        saveData.gain = 90;

        //Convert to Json
        string jsonData = JsonUtility.ToJson(saveData);
        //Save Json string
        File.WriteAllText(keyingDataFilePath, jsonData);
    }
}

[SerializeField]
public class saveChromakeyData
{
    public Color keyColor = Color.green;
    public float range = 0;
    public float smoothness = 0;
    public float whiteClip = 0;
    public float blackClip = 0;
    public int erosion = 0;
    public float despill = 0;
    public int exposure = 0;
    public int gain = 0;
}