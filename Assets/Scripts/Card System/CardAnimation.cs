using UnityEngine;
using System.Collections;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] private Vector2 _deckPilePosition;
    [SerializeField] private Vector2 _discardPilePosition;
    [SerializeField] private Vector2 _handSpacePosition;
    [SerializeField] private Vector2 _screenCenter;

    [SerializeField] private float _animationDuration;

    private RectTransform _rectTransform;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public IEnumerator DrawCard()
    {
        yield return StartCoroutine(MoveCard(_deckPilePosition, _handSpacePosition));
    }
    public IEnumerator DiscardCard()
    {
        yield return StartCoroutine(MoveCard(_handSpacePosition, _discardPilePosition));
    }
    public IEnumerator AddToHand()
    {
        yield return StartCoroutine(MoveCard(_screenCenter, _handSpacePosition));
    }

    private IEnumerator MoveCard(Vector2 startPos, Vector2 endPos)
    {
        float progress = 0;

        while (progress <= 1)
        {
            _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);
            progress += Time.deltaTime / _animationDuration;
            yield return null;
        }

        _rectTransform.anchoredPosition = endPos;
    }
}
