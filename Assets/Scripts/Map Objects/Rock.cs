using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class Rock : MapObject
{
    [SerializeField] Sprite _root1X1Sprite;

    private void Awake()
    {
        _entityType = EntityType.Rock;
    }

    public override void DestroyObjectAt(Coords coords)
    {
        base.DestroyObjectAt(coords);

        Transform sprite = transform.Find("Sprite");
        if (sprite != null)
            Destroy(sprite.gameObject);

        List<Coords> remainingObjects = GetAllCoords();
        foreach (Coords coord in remainingObjects)
        {
            GameObject smallRock = GetObjectAt(coord);
            smallRock.GetComponent<SpriteRenderer>().sprite = _root1X1Sprite;
        }
    }
}
