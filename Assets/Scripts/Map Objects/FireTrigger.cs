using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class FireTrigger : MapObject
{
    [SerializeField] private List<Fire> _relatedFires;
    private List<List<Coords>> _newTrajectories;

    [SerializeField] private List<Coords> _trajectory1;
    [SerializeField] private List<Coords> _trajectory2;


    [SerializeField] Sprite _activeSprite;


    private void Awake()
    {
        _entityType = EntityType.FireTrigger;

        _newTrajectories = new List<List<Coords>>
        {
            _trajectory1,
            _trajectory2
        };
    }

    public override void RunRootInteractionProcess(params RootBlock[] rootBlocks)
    {
        GetComponent<SpriteRenderer>().sprite = _activeSprite;

        for (int i = 0; i < _relatedFires.Count; i++)
            _relatedFires[i].SetNewTrajectory(_newTrajectories[i]);
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        GetComponent<SpriteRenderer>().sprite = _activeSprite;

        for (int i = 0; i < _relatedFires.Count; i++)
            _relatedFires[i].SetNewTrajectory(_newTrajectories[i]);
    }
}
