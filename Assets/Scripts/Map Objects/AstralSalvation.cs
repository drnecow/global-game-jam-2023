using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class AstralSalvation : MapObject
{
    private void Awake()
    {
        _entityType = EntityType.AstralSalvation;
    }

    public override void RunRootInteractionProcess(params RootBlock[] rootBlocks)
    {
        GameEventSystem.Instance.OnWinGame.Invoke();
    }
}
