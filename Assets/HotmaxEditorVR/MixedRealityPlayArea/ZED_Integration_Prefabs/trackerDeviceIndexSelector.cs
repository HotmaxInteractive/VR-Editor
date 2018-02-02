using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class trackerDeviceIndexSelector : MonoBehaviour
{

    private SteamVR_TrackedObject trackedObject;

    public Text debugTextComponent;

    private string alphaKey;

    private bool selectDeviceIndexMode = true;

    private void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        trackedObject.index = SteamVR_TrackedObject.EIndex.Device5;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            selectDeviceIndexMode = !selectDeviceIndexMode;

            if (selectDeviceIndexMode)
            {
                updateText("ON");
            }
            else
            {
                updateText("OFF");
            }
        }

        if (selectDeviceIndexMode)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    alphaKey = vKey.ToString();
                    switch (alphaKey)
                    {
                        case "Alpha1":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device1;
                            updateText("Alpha1");
                            break;
                        case "Alpha2":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device2;
                            updateText("Alpha2");
                            break;
                        case "Alpha3":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device3;
                            updateText("Alpha3");
                            break;
                        case "Alpha4":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device4;
                            updateText("Alpha4");
                            break;
                        case "Alpha5":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device5;
                            updateText("Alpha5");
                            break;
                        case "Alpha6":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device6;
                            updateText("Alpha6");
                            break;
                        case "Alpha7":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device7;
                            updateText("Alpha7");
                            break;
                        case "Alpha8":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device8;
                            updateText("Alpha8");
                            break;
                        case "Alpha9":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device9;
                            updateText("Alpha9");
                            break;
                        case "Q":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device10;
                            updateText("Alpha10");
                            break;
                        case "W":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device11;
                            updateText("Alpha11");
                            break;
                        case "E":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device12;
                            updateText("Alpha12");
                            break;
                        case "R":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device13;
                            updateText("Alpha13");
                            break;
                        case "T":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device14;
                            updateText("Alpha14");
                            break;
                        case "Y":
                            trackedObject.index = SteamVR_TrackedObject.EIndex.Device15;
                            updateText("Alpha15");
                            break;
                    }
                }
            }
        }
    }

    void updateText(string text)
    {
        debugTextComponent.text = text;
        Invoke("clearDebugText", 1);
    }

    void clearDebugText()
    {
        debugTextComponent.text = "";
    }
}
