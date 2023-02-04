using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class CardPile : MonoBehaviour
{
    List<CardType> _cards;


    private void Awake()
    {
        _cards = new List<CardType>();
    }

    private void Shuffle()
    {

    }
    public List<CardType> Draw(int cardNumber)
    {
        List<CardType> drawnCards = new List<CardType>();

        for (int i = 0; i < Mathf.Min(cardNumber, _cards.Count + 1); i++)
        {
            drawnCards.Add(_cards[_cards.Count - 1]);
            _cards.RemoveAt(_cards.Count - 1);
        }

        return drawnCards;
    }
}
