using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSource
{
    private List<RootBlock> _affectedRoots;

    public FireSource()
    {
        _affectedRoots = new List<RootBlock>();
    }

    public void Spread()
    {

    }

    public void AddRootBlock(RootBlock rootBlock)
    {
        if (!_affectedRoots.Contains(rootBlock))
            _affectedRoots.Add(rootBlock);
        else
            Debug.Log($"Fire source {this} already affects root block {rootBlock}");
    }
}
