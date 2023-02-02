using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class Water : MapObject
{
    private void Awake()
    {
        _entityType = EntityType.Water;
    }

    public override void RunRootInteractionProcess(params RootBlock[] rootBlocks)
    {
        base.RunRootInteractionProcess(rootBlocks);
    }
}
