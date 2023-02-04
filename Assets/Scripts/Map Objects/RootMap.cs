using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMap : MonoBehaviour
{
    public static RootMap Instance { get; private set; } // RootMap is a Singleton
    public RootBlock[,] Roots { get => _roots; }

    private RootBlock[,] _roots;


    private void Awake()
    {
        // Setup Singleton behavior
        if (Instance != null && Instance != this)
        {
            Debug.Log("RootMap instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;
    }
    private void Start()
    {
        _roots = new RootBlock[Map.Instance.Width, Map.Instance.Height];
        //Debug.Log($"Root map: width — {_roots.GetLength(0)}, height — {_roots.GetLength(1)}");
    }

    public void AddRootBlock(Coords coords, RootBlock rootBlock)
    {
        if (Map.Instance.ValidateCoords(coords))
        {
            _roots[coords.x, coords.y] = rootBlock;
            //Debug.Log($"Root block {rootBlock} added at coordinates ({coords.x}, {coords.y})");
        }
        else
            Debug.Log($"Invalid coordinates given: ({coords.x}, {coords.y})");
    }
    public void RemoveRootBlockAt(Coords coords)
    {
        if (Map.Instance.ValidateCoords(coords))
        {
            _roots[coords.x, coords.y] = null;
            Debug.Log($"Root block removed from coordinates ({coords.x}, {coords.y})");
        }
    }

    public bool IsEmpty(List<Coords> coords)
    {
        if (coords.Count > 0)
        {
            foreach (Coords coord in coords)
            {
                if (Map.Instance.ValidateCoords(coord) && _roots[coord.x, coord.y] != null)
                    return false;
            }

            return true;
        }

        return true;
    }
    public List<Coords> GetEmpty(List<Coords> coords)
    {
        List<Coords> emptyCoords = new List<Coords>();

        foreach (Coords coord in coords)
            if (Map.Instance.ValidateCoords(coord))
                if (_roots[coord.x, coord.y] == null)
                    emptyCoords.Add(coord);

        return emptyCoords;
    }
    public List<Coords> GetNonEmpty(List<Coords> coords)
    {
        List<Coords> nonEmptyCoords = new List<Coords>();

        foreach (Coords coord in coords)
            if (Map.Instance.ValidateCoords(coord))
                if (_roots[coord.x, coord.y] != null)
                    nonEmptyCoords.Add(coord);

        return nonEmptyCoords;
    }
}
