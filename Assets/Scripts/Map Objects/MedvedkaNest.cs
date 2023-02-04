using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class MedvedkaNest : MapObject
{
    [SerializeField] private GameObject _spawnedMedvedka;
    [SerializeField] private int _spawnRate;
    private int _turnsUntilNextSpawn = 0;


    private void Awake()
    {
        _entityType = EntityType.MedvedkaNest;
    }

    public void SpawnMedvedka()
    {
        if (_turnsUntilNextSpawn > 0)
            _turnsUntilNextSpawn--;
        else
        {
            List<Coords> neighbours = Map.Instance.GetNeighbours(CurrentCoords, Constants.NEIGHBOURS_2X2);
            List<Coords> possibleSpawnCoords = new List<Coords>();

            foreach (Coords neighbour in neighbours)
            {
                MapObject obj = Map.Instance.GridArray[neighbour.x, neighbour.y];

                if (obj == null)
                    possibleSpawnCoords.Add(neighbour);
            }

            if (possibleSpawnCoords.Count > 0)
            {
                int randomDir = Random.Range(0, possibleSpawnCoords.Count);
                GameObject newMedvedka = Instantiate(_spawnedMedvedka);

                newMedvedka.transform.position = Map.Instance.XYToWorldPos(possibleSpawnCoords[randomDir]);

                Map.Instance.SetExistingObjectCoords(newMedvedka.GetComponent<MapObject>());
                GameController.Instance.AddMedvedka(newMedvedka.GetComponent<Medvedka>());
            }

            _turnsUntilNextSpawn = _spawnRate - 1;
        }
    }

    public override void DestroyThis()
    {
        GameController.Instance.RemoveMedvedkaNest(this);
        base.DestroyThis();
    }
}
