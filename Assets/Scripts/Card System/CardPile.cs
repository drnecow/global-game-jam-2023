using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class CardPile
{
    List<CardType> _cards;
    public int Size { get { return _cards.Count; } }
    public List<CardType> Cards { get => _cards; }


    public CardPile()
    {
        _cards = new List<CardType>();
    }

    public void Shuffle()
    {
        int n = _cards.Count;

        while (n > 1)
        {
            n--;

            int randomIndex = Random.Range(0, n + 1);

            CardType card = _cards[randomIndex];
            _cards[randomIndex] = _cards[n];
            _cards[n] = card;
        }
    }
    public void AddCard(CardType card)
    {
        _cards.Add(card);
    }
    public void ReplaceCardsWith(List<CardType> newCards)
    {
        if (newCards != null)
            _cards = newCards;
    }
    public List<CardType> Draw(int cardNumber)
    {
        List<CardType> drawnCards = new List<CardType>();

        if (cardNumber < _cards.Count)
        {
            for (int i = 0; i < cardNumber; i++)
            {
                drawnCards.Add(_cards[_cards.Count - 1]);
                _cards.RemoveAt(_cards.Count - 1);
            }
        }
        else
        {
            foreach (CardType card in _cards)
                drawnCards.Add(card);

            _cards.Clear();
        }

        return drawnCards;
    }
}
