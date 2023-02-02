using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class CosmicTree : MapObject
{
    private void Awake()
    {
        _entityType = EntityType.Tree;
    }
}
