using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class WatchTower : MapObject
{
    private void Awake()
    {
        _entityType = EntityType.WatchTower;
    }

    public override void RunRootInteractionProcess(params RootBlock[] rootBlocks)
    {
        List<Coords> neighbours5 = new List<Coords>();

        for (int i = -5; i < 6; i++)
            for (int j = -5; j < 6; j++)
                neighbours5.Add(new Coords(CurrentCoords.x + i, CurrentCoords.y + j));

        FogOfWar.Instance.RemoveSquaresAt(neighbours5);

        Animator animator = GetComponent<Animator>();
        animator.SetBool("Activated", true);
    }
}
