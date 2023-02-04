using UnityEngine;
using System.Collections;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform deckPile;
    [SerializeField] private RectTransform discardPile;
    [SerializeField] private RectTransform handSpace;
    [SerializeField] private float animationDuration = 0.5f;

    private Vector2 deckPileStart;
    private Vector2 handSpaceEnd;
    private Vector2 discardPileEnd;

    private void Start()
    {
        deckPileStart = deckPile.anchoredPosition;
        handSpaceEnd = handSpace.anchoredPosition + new Vector2(handSpace.rect.width / 2, 0);
        discardPileEnd = discardPile.anchoredPosition;
    }

    [ContextMenu("Draw")]
    public void DrawCard()
    {
        StartCoroutine(MoveCard(deckPileStart, handSpaceEnd));
    }

    [ContextMenu("Discard")]
    public void DiscardCard()
    {
        StartCoroutine(MoveCard(handSpaceEnd, discardPileEnd));
    }

    private IEnumerator MoveCard(Vector2 startPos, Vector2 endPos)
    {
        float progress = 0;

        while (progress <= 1)
        {
            (transform as RectTransform).anchoredPosition = Vector2.Lerp(startPos, endPos, progress);
            progress += Time.deltaTime / animationDuration;
            yield return null;
        }

        (transform as RectTransform).anchoredPosition = endPos;
    }
}
