using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class CardSystem : MonoBehaviour
{
    CardPile _deck;
    CardPile _discard;

    List<CardType> _hand;
    List<GameObject> _handPrefabs;


    private void Awake()
    {
        _hand = new List<CardType>();
    }

    public void StartGame()
    {
        DrawNewTurnCards();
    }
    public void NewTurn()
    {
        // Draw cards from deck, 4 or until it's empty 
    }
    public void EndTurn()
    {
        // Discard hand, except persistent cards
        // If the deck is empty, shuffle discard and fill the deck with it
    }
    public void Tap()
    {
        // -1 to turn timer
        // If the deck isn't empty, draw one card
    }

    private void DiscardHand()
    {

    }
    private void DrawNewTurnCards()
    {
        _hand = _deck.Draw(4);
    }
    private void AnimateDraw(List<CardType> cards)
    {
        foreach (CardType card in cards)
        {

        }
    }
}
