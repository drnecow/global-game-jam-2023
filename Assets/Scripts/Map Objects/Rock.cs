using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class Rock : MapObject
{
    private void Awake()
    {
        _entityType = EntityType.Rock;
    }
}
