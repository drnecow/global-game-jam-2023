using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class Root : MapObject
{
    private bool _isDrilling;

    public bool IsDrilling { get => _isDrilling; set => _isDrilling = value; }


    private void Awake()
    {
        _entityType = EntityType.Root;
    }

    public void DestroyRootBlockAt(Coords coords)
    {
        DestroyObjectAt(coords);

        // Check for detached roots and destroy them as well
    }
}
