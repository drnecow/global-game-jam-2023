using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetup : MonoBehaviour
{
    public static MapSetup Instance { get; private set; } // MapSetup is a Singleton

    [SerializeField] List<GameObject> _multipleObjects;
    [SerializeField] List<GameObject> _singularObjects;


    private void Awake()
    {
        // Setup Singleton behavior
        if (Instance != null && Instance != this)
        {
            Debug.Log("MapSetup instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;
    }

    [ContextMenu("Populate Map")]
    public void PopulateMap()
    {
        foreach (GameObject multObj in _multipleObjects)
            SetObjectGroup(multObj);

        foreach (GameObject singObj in _singularObjects)
            Map.Instance.SetExistingObjectCoords(singObj.GetComponent<MapObject>());

        //Map.Instance.PrintGrid();
    }

    private void SetObjectGroup(GameObject gameObj)
    {
        List<MapObject> mapObjects = new List<MapObject>();

        foreach (Transform child in gameObj.transform)
            mapObjects.Add(child.gameObject.GetComponent<MapObject>());

        foreach (MapObject obj in mapObjects)
            Map.Instance.SetExistingObjectCoords(obj);
    }
}
