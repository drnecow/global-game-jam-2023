using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetup : MonoBehaviour
{
    [SerializeField] GameObject _tree;
    [SerializeField] GameObject _rocks;
    [SerializeField] GameObject _rockTrigger;
    [SerializeField] GameObject _fire;
    [SerializeField] GameObject _water;
    [SerializeField] GameObject _medvedkaNests;
    [SerializeField] GameObject _astralSalvation;

    [ContextMenu("Populate Map")]
    public void PopulateMap()
    {
        Map.Instance.SetExistingObjectCoords(_tree.GetComponent<MapObject>());

        SetObjectGroup(_rocks);
        Map.Instance.SetExistingObjectCoords(_rockTrigger.GetComponent<MapObject>());

        SetObjectGroup(_fire);

        SetObjectGroup(_water);

        SetObjectGroup(_medvedkaNests);

        Map.Instance.SetExistingObjectCoords(_astralSalvation.GetComponent<MapObject>());

        Map.Instance.PrintGrid();
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
