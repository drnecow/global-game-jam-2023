using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform _rectTransform;
    private float _returnHeight = 240f;

    Card _card;

    Transform _parentAfterDrag;
    private Vector2 _originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        _parentAfterDrag = transform.parent;
        _originalPosition = GetComponent<RectTransform>().anchoredPosition;
        //transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");

        if (_rectTransform.anchoredPosition.y < _returnHeight && GameController.Instance.RootBeingPlaced)
        {
            Debug.Log("Card below return height");
            _rectTransform.anchoredPosition = _originalPosition;
        }
        else
        {
            Debug.Log("Card above return height");
            _card.PlayCard();
        }
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _card = GetComponent<Card>();
    }
}
