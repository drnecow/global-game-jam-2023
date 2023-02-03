using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class RockTrigger : MapObject
{
    [SerializeField] private Rock _relatedRock;
    [SerializeField] private Coords _newRockCoords;

    [SerializeField] private MedvedkaNest _relatedMedvedkaNest;

    private void Awake()
    {
        _entityType = EntityType.RockTrigger;
    }

    public override void RunRootInteractionProcess(params RootBlock[] rootBlocks)
    {
        Map.Instance.ClearExistingObjectCoords(_relatedRock);
        _relatedRock.gameObject.transform.position = Map.Instance.XYToWorldPos(_newRockCoords);
        Map.Instance.SetExistingObjectCoords(_relatedRock);

        Map.Instance.ClearExistingObjectCoords(_relatedMedvedkaNest);
        Destroy(_relatedMedvedkaNest);
    }

    [ContextMenu("Smash Medvedka Nest")]
    public void RunRootInteractionProcess()
    {
        Map.Instance.ClearExistingObjectCoords(_relatedRock);
        _relatedRock.gameObject.transform.position = Map.Instance.XYToWorldPos(_newRockCoords);
        Map.Instance.SetExistingObjectCoords(_relatedRock);

        Map.Instance.ClearExistingObjectCoords(_relatedMedvedkaNest);
        Destroy(_relatedMedvedkaNest.gameObject);
    }
}
