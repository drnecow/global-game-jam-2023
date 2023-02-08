using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class RockTrigger : MapObject
{
    [SerializeField] private Rock _relatedRock;
    [SerializeField] private Coords _newRockCoords;

    [SerializeField] private MedvedkaNest _relatedMedvedkaNest;


    [SerializeField] Sprite _activeSprite;


    private void Awake()
    {
        _entityType = EntityType.RockTrigger;
    }

    public override void RunRootInteractionProcess(params RootBlock[] rootBlocks)
    {
        GetComponent<SpriteRenderer>().sprite = _activeSprite;

        Map.Instance.ClearExistingObjectCoords(_relatedMedvedkaNest);
        _relatedMedvedkaNest.DestroyThis();

        Map.Instance.ClearExistingObjectCoords(_relatedRock);
        CoroutineAnimation.Instance.MoveTo(_relatedRock.gameObject, Map.Instance.XYToWorldPos(_newRockCoords));
        //_relatedRock.gameObject.transform.position = Map.Instance.XYToWorldPos(_newRockCoords);
        //Map.Instance.SetExistingObjectCoords(_relatedRock);
        Map.Instance.SetOccupied(_newRockCoords, _relatedRock);
    }
}
