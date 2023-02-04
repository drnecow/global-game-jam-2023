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


    public void MakeFireProof()
    {
        List<Coords> coords = GetAllCoords();

        foreach (Coords coord in coords)
        {
            GameObject part = GetObjectAt(coord);
            RootBlock rootBlock = part.GetComponent<RootBlock>();

            rootBlock.IsFireProof = true;
        }
    }
    public void MakeHardened()
    {
        List<Coords> coords = GetAllCoords();

        foreach (Coords coord in coords)
        {
            GameObject part = GetObjectAt(coord);
            RootBlock rootBlock = part.GetComponent<RootBlock>();

            rootBlock.IsHardened = true;
        }
    }
}
