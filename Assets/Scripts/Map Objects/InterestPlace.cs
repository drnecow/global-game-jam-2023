using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class InterestPlace : MapObject
{
    [SerializeField] private List<CardType> _cards;

    private void Awake()
    {
        _entityType = EntityType.InterestPlace;
    }

    public override void RunRootInteractionProcess(params RootBlock[] rootBlocks)
    {
        Debug.Log("Interacted");
        int randomIndex = Random.Range(0, _cards.Count);
        CardSystem.Instance.AddToHand(new List<CardType>() { _cards[randomIndex] });
    }
}
