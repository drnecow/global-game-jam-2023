using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class Card : MonoBehaviour
{
    Action _action;
    [SerializeField] CardType _cardType;

    [SerializeField] private bool _discardable; // Whether or not card goes to discard pile when it's played
    [SerializeField] private bool _persistent; // Whether or not card stays in hand at the start of each turn

    public bool Discardable { get => _discardable; }
    public bool Persistent { get => _persistent; }
    public CardType CardType { get => _cardType; }

    private void Awake()
    {
        _action = Constants.CARD_ACTIONS[_cardType];
    }

    public void PlayCard()
    {
        _action();
        CardSystem.Instance.OnCardPlayed.Invoke(gameObject);
    }

    [ContextMenu("Position And Rotation")]
    public void Info()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Debug.Log(rectTransform.anchoredPosition);
        Debug.Log(rectTransform.rotation);
    }
}
