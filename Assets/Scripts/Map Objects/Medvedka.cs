using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class Medvedka : MapObject
{
    [SerializeField] private int _eatingTimer;
    public int EatingTimer { get => _eatingTimer; }


    private void Awake()
    {
        _entityType = EntityType.Medvedka;
    }

    public void MoveRandomly()
    {
        List<Coords> neighbours = Map.Instance.GetNeighbours(CurrentCoords, Constants.NEIGHBOURS_1X1);
        List<Coords> moveDirections = new List<Coords>();

        foreach (Coords neighbour in neighbours)
        {
            MapObject obj = Map.Instance.GridArray[neighbour.x, neighbour.y];
            
            if (obj == null)
                moveDirections.Add(neighbour);

            else if (obj.EntityType == EntityType.Root && obj)
            {
                RootBlock objRootBlock = obj.GetObjectAt(neighbour).GetComponent<RootBlock>();

                if (!objRootBlock.IsHardened && !objRootBlock.IsBeingEaten && !objRootBlock.IsBurning)
                {
                    EatRoot(objRootBlock);
                    return;
                }
            }
        }

        if (moveDirections.Count > 0)
        {
            int randomDir = Random.Range(0, moveDirections.Count);

            Map.Instance.FreeCoords(CurrentCoords);

            CoroutineAnimation.Instance.MoveTo(gameObject, Map.Instance.XYToWorldPos(moveDirections[randomDir]));
            //transform.position = Map.Instance.XYToWorldPos(moveDirections[randomDir]);
            //Map.Instance.SetExistingObjectCoords(this);
            Map.Instance.SetOccupied(moveDirections[randomDir], this);
        }
    }

    private void EatRoot(RootBlock root)
    {
        Debug.Log("Medvedka starts eating root");

        Map.Instance.FreeCoords(CurrentCoords);
        //CoroutineAnimation.Instance.MoveTo(gameObject, root.transform.position);
        //Map.Instance.SetOccupied(Map.Instance.WorldPosToXY(root.transform.position), this);
        transform.position = root.transform.position;
        GetComponent<Animator>().SetBool("Eating", true);

        root.BeginBeingEaten(this);
        GameController.Instance.AddEatingMedvedkaToRemove(this);
    }

    public override void DestroyThis()
    {
        GameController.Instance.KillMedvedka(this);
        base.DestroyThis();
    }
}
