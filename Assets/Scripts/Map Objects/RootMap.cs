using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMap : MonoBehaviour
{
    public static RootMap Instance { get; private set; } // RootMap is a Singleton

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

    public void AddRootBlock(RootBlock rootBlock)
    {
        Coords blockCoords = Map.Instance.WorldPosToXY(rootBlock.gameObject.transform.position);

        if (Map.Instance.ValidateCoords(blockCoords))
        {
            _roots[blockCoords.x, blockCoords.y] = rootBlock;
            Debug.Log($"Root block {rootBlock} added at coordinates ({blockCoords.x}, {blockCoords.y})");
        }
        else
            Debug.Log($"Invalid coordinates given: ({blockCoords.x}, {blockCoords.y})");
    }
    // Is it needed if the RootBlock at coords already gets destroyed in Root?..
    //public void EmptyCellAt(Coords coords)
    //{
    //    if (Map.Instance.ValidateCoords(coords))
    //        _roots[coords.x, coords.y] = null;
    //    else
    //        Debug.Log($"Invalid coordinates given: ({coords.x}, {coords.y})");
    //}
}
