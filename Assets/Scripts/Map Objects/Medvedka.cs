using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class Medvedka : MapObject
{
    [SerializeField] private int _eatingTimer;


    private void Awake()
    {
        _entityType = EntityType.Medvedka;
    }

    public void MoveRandomly()
    {
        List<Coords> possibleDirections = new List<Coords>();

        foreach (Vector2 dir in Constants.NEIGHBOURS_1X1)
        {
            Coords neighbourCoords = new Coords(CurrentCoords.x + (int)dir.x, CurrentCoords.y + (int)dir.y);

            if (Map.Instance.ValidateCoords(neighbourCoords))
            {
                MapObject obj = Map.Instance.GridArray[neighbourCoords.x, neighbourCoords.y];

                if (obj == null)
                    possibleDirections.Add(neighbourCoords);
                else if (obj.EntityType == EntityType.Root)
                {
                    EatRoot();
                    return;
                }
            }
        }

        if (possibleDirections.Count > 0)
        {
            int randomDir = Random.Range(0, possibleDirections.Count);

            Map.Instance.FreeCoords(CurrentCoords);

            transform.position = Map.Instance.XYToWorldPos(possibleDirections[randomDir]);
            Map.Instance.SetExistingObjectCoords(this);
        }
    }

    private void EatRoot()
    {
        Debug.Log("Medvedka starts eating root");
    }

    public override void DestroyThis()
    {
        GameController.Instance.RemoveMedvedka(this);
        Debug.Log($"Destroying {this} with its GameObject");
        Destroy(gameObject);
    }
}
