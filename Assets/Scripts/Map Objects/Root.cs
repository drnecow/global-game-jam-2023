using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class Root : MapObject
{
    [SerializeField] private Sprite _fireProof;
    [SerializeField] private Sprite _hardened;


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
            part.GetComponent<SpriteRenderer>().sprite = _fireProof;

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
            part.GetComponent<SpriteRenderer>().sprite = _hardened;

            rootBlock.IsHardened = true;
        }
    }

    public override void DestroyObjectAt(Coords coords)
    {
        GameObject obj = GetObjectAt(coords);

        if (obj != null)
        {
            RootBlock rootBlock = obj.GetComponent<RootBlock>();

            if (rootBlock.Eater != null)
            {
                GameController.Instance.AddMedvedka(rootBlock.Eater);
                Map.Instance.SetExistingObjectCoords(rootBlock.Eater);
                rootBlock.Eater.GetComponent<Animator>().SetBool("Eating", false);
            }

            int objIndex = _cells.IndexOf(obj);

            _cells.RemoveAt(objIndex);
            _cellDisplacements.RemoveAt(objIndex);
            Destroy(obj);
            Map.Instance.FreeCoords(coords);

            // If MapObject doesn't have any contents, it is no longer needed
            if (_cells.Count == 0 && _cellDisplacements.Count == 0)
                Destroy(this);
        }
    }
}
