using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class Root : MapObject
{
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
