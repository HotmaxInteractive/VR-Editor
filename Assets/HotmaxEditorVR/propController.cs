using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class propController : MonoBehaviour
{
    //--Lists to handle getting unique props in menu
    private List<string> propNames = new List<string>();
    public static List<GameObject> propObjects = new List<GameObject>();

    void Start()
    {
        //get the names of all props in props
        //TODO: we will be getting the props from a propClass in the form of a List of Serialized GameObjects
        for (int i = 0; i < transform.childCount; i++)
        {
            propNames.Add(transform.GetChild(i).gameObject.name);
        }

        //weed out props that have the same name
        propNames = propNames.Distinct().ToList();

        //copy the game objects with the unique names to a gameObject list 
        for (int i = 0; i < propNames.Count; i++)
        {
            propObjects.Add(GameObject.Find(propNames[i]));
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
