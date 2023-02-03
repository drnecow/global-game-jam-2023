using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public abstract class MapObject : MonoBehaviour
{
    protected EntityType _entityType;
    public EntityType EntityType
    {
        get { return _entityType; }
    }
    public Coords CurrentCoords
    {
        get { return Map.Instance.WorldPosToXY(transform.position); }
    }

    [SerializeField] protected List<GameObject> _cells;
    [SerializeField] protected List<Vector2> _cellDisplacements;


    public virtual void RunRootInteractionProcess(params RootBlock[] rootBlocks)
    {
    }


    public GameObject GetObjectAt(Coords coords)
    {
        int xDisplacement = coords.x - CurrentCoords.x;
        int yDisplacement = coords.y - CurrentCoords.y;

        Vector2 displVector = new Vector2(xDisplacement, yDisplacement);

        int rotation = (int)transform.eulerAngles.z;


        Debug.Log($"Rotation: {rotation}");

        // Transform the displacement vector based on the rotation
        if (rotation != 0 && rotation != 90 && rotation != 180 && rotation != 270)
        {
            Debug.Log($"Rotation isn't equal to either 0, 90, 180, or 270: {rotation}, can't get object");
            return null;
        }
        else if (rotation == 270) // equals -90 in Inspector
            displVector = new Vector2(displVector.y, -displVector.x);
        else if (rotation == 180) // equals -180 in Inspector
            displVector = new Vector2(-displVector.x, -displVector.y);
        else if (rotation == 90) // equals -270 in Inspector
            displVector = new Vector2(-displVector.y, displVector.x);

        // Find object corresponding to the displacement vector
        for (int i = 0; i < _cellDisplacements.Count; i++)
            if (_cellDisplacements[i] == displVector)
            {
                Debug.Log($"Displacement of object at coordinates ({coords.x}, {coords.y}) is {displVector}");
                return _cells[i];
            }

        Debug.Log($"Object at coordinates ({coords.x}, {coords.y}) doesn't belong to this MapObject");
        return null;
    }
    protected void DestroyObjectAt(Coords coords)
    {
        GameObject obj = GetObjectAt(coords);
        
        if (obj != null)
        {
            int objIndex = _cells.IndexOf(obj);

            _cells.RemoveAt(objIndex);
            _cellDisplacements.RemoveAt(objIndex);
            Destroy(obj);
            Map.Instance.FreeCoords(coords);

            // If MapObject doesn't have any contents, it is no longer needed
            if (_cells.Count == 0 && _cellDisplacements.Count == 0)
                Destroy(this);
        }
    }

    public virtual void DestroyThis()
    {
        Debug.Log($"Destroying {this} with its GameObject");
        Destroy(gameObject);
    }

    public List<Coords> GetAllCoords()
    {
        List<Coords> coords = new List<Coords>();

        foreach (GameObject obj in _cells)
            coords.Add(Map.Instance.WorldPosToXY(obj.transform.position));

        return coords;
    }

    
}
