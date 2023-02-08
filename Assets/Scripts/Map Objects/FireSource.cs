using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class FireSource
{
    private List<RootBlock> _affectedRoots;

    public FireSource()
    {
        _affectedRoots = new List<RootBlock>();
        GameEventSystem.Instance.OnRootBlockDestroyed.AddListener((rootBlock) => RemoveRootBlock(rootBlock));
    }

    public void Spread()
    {
        List<Coords> possibleSpreadTargets = new List<Coords>();

        foreach (RootBlock root in _affectedRoots) {
            if (root != null)
            {
                List<Coords> neighbours = Map.Instance.GetNeighbours(Map.Instance.WorldPosToXY(root.transform.position), Constants.NEIGHBOURS_1X1);

                foreach (Coords neighbour in neighbours)
                {
                    RootBlock neighbourBlock = RootMap.Instance.Roots[neighbour.x, neighbour.y];

                    if (neighbourBlock != null && !neighbourBlock.IsBurning && !neighbourBlock.IsFireProof && !neighbourBlock.IsImmersedInWater)
                        possibleSpreadTargets.Add(neighbour);
                }
            }
        }

        if (possibleSpreadTargets.Count > 0)
        {
            int target = Random.Range(0, possibleSpreadTargets.Count);
            Coords targetCoords = possibleSpreadTargets[target];

            RootBlock targetRootBlock = RootMap.Instance.Roots[targetCoords.x, targetCoords.y];
            targetRootBlock.SetOnFire();
            _affectedRoots.Add(targetRootBlock);
        }
    }

    public void AddRootBlock(RootBlock rootBlock)
    {
        if (!_affectedRoots.Contains(rootBlock))
            _affectedRoots.Add(rootBlock);
        else
            Debug.Log($"Fire source {this} already affects root block {rootBlock}");
    }
    private void RemoveRootBlock(RootBlock rootBlock)
    {
        if (_affectedRoots.Contains(rootBlock))
        {
            _affectedRoots.Remove(rootBlock);

            if (_affectedRoots.Count == 0)
                GameController.Instance.RemoveFireSource(this);
        }
        else
            Debug.Log($"Fire source {this} doesn't affect root block {rootBlock}");
    }
}
