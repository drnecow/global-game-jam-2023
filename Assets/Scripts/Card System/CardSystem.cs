using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Project.Constants;

public class CardSystem : MonoBehaviour
{
    public static CardSystem Instance { get; private set; }

    List<CardType> _startingCards = new List<CardType>() { CardType.PlaceLine, CardType.PlaceZigzag, CardType.PlaceOutgrowthLine, CardType.PlaceCorner, CardType.PlaceCross };

    CardPile _deck;
    CardPile _discard;

    List<GameObject> _handPrefabs;
    [SerializeField] RectTransform _handObject;

    [SerializeField] List<GameObject> _cardPrefabs;
    [SerializeField] private float _startingXDisplacement;
    [SerializeField] private float _totalXDisplacement;

    public UnityEvent<int> OnDeckSizeChanged;
    public UnityEvent<int> OnDiscardSizeChanged;
    public UnityEvent<GameObject> OnCardPlayed;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("CardSystem instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;


        _deck = new CardPile();
        _discard = new CardPile();

        _handPrefabs = new List<GameObject>();

        OnCardPlayed.AddListener((cardPrefab) => RemoveFromHand(cardPrefab));
    }


    public void StartGame()
    {
        foreach (CardType cardType in _startingCards)
        {
            for (int i = 0; i < 3; i++)
                _deck.AddCard(cardType);
        }
        _deck.Shuffle();
    }
    public void NewTurn()
    {
        // Draw cards from deck, 4 or until it's empty 
        Debug.Log($"New turn: deck size -- {_deck.Size}");
        DrawNewTurnCards();
    }
    public void EndTurn()
    {
        Debug.Log($"End turn: deck size -- {_deck.Size}");
        // Discard hand, except persistent cards
        // If the deck is empty, shuffle discard and fill the deck with it
        DiscardHand();

        if (_deck.Size == 0)
        {
            _discard.Shuffle();

            _deck.ReplaceCardsWith(_discard.Cards);
            OnDeckSizeChanged.Invoke(_deck.Size);
            _discard.ReplaceCardsWith(new List<CardType>() { });
            OnDiscardSizeChanged.Invoke(_discard.Size);
        }
    }
    public void Tap()
    {
        // -1 to turn timer
        // If the deck isn't empty, draw one card
        GameController.Instance.DecrementTurnTimer();

        List<CardType> drawnCard = _deck.Draw(1);
        OnDeckSizeChanged.Invoke(_deck.Size);
        StartCoroutine(DrawCards(drawnCard));
    }
    public void AddToHand(List<CardType> cards)
    {
        StartCoroutine(AddCards(cards));
    }
    public void RemoveFromHand(GameObject cardPrefab)
    {
        Card card = cardPrefab.GetComponent<Card>();
        _handPrefabs.Remove(cardPrefab);

        if (card.Discardable)
        {
            _discard.AddCard(card.CardType);
            OnDiscardSizeChanged.Invoke(_discard.Size);
        }

        Destroy(cardPrefab);
        RearrangeHand();
    }

    private void DiscardHand()
    {
        List<GameObject> toDiscard = new List<GameObject>();

        foreach (GameObject cardPrefab in _handPrefabs)
        {
            Card card = cardPrefab.GetComponent<Card>();

            if (!card.Persistent)
            {
                toDiscard.Add(cardPrefab);
                _discard.AddCard(card.CardType);
            }
        }

        OnDiscardSizeChanged.Invoke(_discard.Size);

        foreach (GameObject discardCard in toDiscard)
            _handPrefabs.Remove(discardCard);

        StartCoroutine(DiscardCards(toDiscard));
    }
    private void DrawNewTurnCards()
    {
        Debug.Log($"Deck size before drawing: {_deck.Size}");
        List<CardType> _handCards = _deck.Draw(4);
        OnDeckSizeChanged.Invoke(_deck.Size);
        StartCoroutine(DrawCards(_handCards));
        Debug.Log($"Deck size after drawing: {_deck.Size}");
    }
    private IEnumerator DrawCards(List<CardType> cards)
    {
        foreach (CardType card in cards)
        {    
            GameObject cardPrefab = Instantiate(_cardPrefabs[(int)card - 1]);
            _handPrefabs.Add(cardPrefab);
            RectTransform rectTransform = cardPrefab.GetComponent<RectTransform>();
            rectTransform.SetParent(_handObject);

            CardAnimation anim = cardPrefab.GetComponent<CardAnimation>();
            yield return StartCoroutine(anim.DrawCard());
            RearrangeHand();
        }
    }
    private IEnumerator DiscardCards(List<GameObject> cardPrefabs)
    {
        foreach (GameObject cardPrefab in cardPrefabs)
        {
            CardAnimation anim = cardPrefab.GetComponent<CardAnimation>();
            yield return StartCoroutine(anim.DiscardCard());

            Destroy(cardPrefab);
        }
    }
    private IEnumerator AddCards(List<CardType> cards)
    {
        foreach (CardType card in cards)
        {
            GameObject cardPrefab = Instantiate(_cardPrefabs[(int)card - 1]);
            _handPrefabs.Add(cardPrefab);
            RectTransform rectTransform = cardPrefab.GetComponent<RectTransform>();
            rectTransform.SetParent(_handObject);

            CardAnimation anim = cardPrefab.GetComponent<CardAnimation>();
            yield return StartCoroutine(anim.AddToHand());
        }

        RearrangeHand();
    }

    private void RearrangeHand()
    {
        //Vector2 startingPoint = Vector2.zero;
        //int halfHand = _handPrefabs.Count / 2;
        //float xDisplacement = (_totalXDisplacement / 2) / halfHand;

        //for (int i = 0; i < _handPrefabs.Count; i++)
        //{
        //    RectTransform cardTransform = _handPrefabs[i].GetComponent<RectTransform>();

        //    if (i < halfHand)
        //    {
        //        cardTransform.anchoredPosition = new Vector2(-(xDisplacement * i), 0);
        //    }
        //    else
        //    {
        //        cardTransform.anchoredPosition = new Vector2(xDisplacement * (i - (halfHand - 1)), 0);
        //    }
        //}

        Vector2 currentXDispl = new Vector2(_startingXDisplacement, 0);
        float xDispl = _totalXDisplacement / _handPrefabs.Count;

        foreach (GameObject cardPrefab in _handPrefabs)
        {
            RectTransform cardTransform = cardPrefab.GetComponent<RectTransform>();
            cardTransform.anchoredPosition = currentXDispl;

            currentXDispl = new Vector2(currentXDispl.x + xDispl, currentXDispl.y);
        }
    }
}
