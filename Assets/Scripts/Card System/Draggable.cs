using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform _rectTransform;
    private float _returnHeight = 240f;

    Card _card;
    private Vector2 _originalPosition;
    private int _siblingIndex;


    public void OnBeginDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, 0);
        _rectTransform.localScale = new Vector3(1f, 1f, 1f);

        _originalPosition = GetComponent<RectTransform>().anchoredPosition;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_rectTransform.anchoredPosition.y < _returnHeight || GameController.Instance.RootBeingPlaced)
        {
            Debug.Log("Card below return height");
            _rectTransform.anchoredPosition = _originalPosition;
            transform.SetSiblingIndex(_siblingIndex);
        }
        else
        {
            Debug.Log("Card above return height");
            _card.PlayCard();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _siblingIndex = transform.GetSiblingIndex();

        transform.SetAsLastSibling();
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, 70);
        _rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, 0);
        _rectTransform.localScale = new Vector3(1f, 1f, 1f);

        transform.SetSiblingIndex(_siblingIndex);
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _card = GetComponent<Card>();
    }
}
