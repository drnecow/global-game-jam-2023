using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _deckCardNumber;
    [SerializeField] private TextMeshProUGUI _discardCardNumber;

    [SerializeField] private Button _endTurnButton;
    [SerializeField] private Button _tapButton;

    private bool _tapUsed = false;


    private void Start()
    {
        CardSystem.Instance.OnDeckSizeChanged.AddListener((newValue) => _deckCardNumber.text = newValue.ToString());
        CardSystem.Instance.OnDiscardSizeChanged.AddListener((newValue) => _discardCardNumber.text = newValue.ToString());

        _endTurnButton.onClick.AddListener(() => GameEventSystem.Instance.OnPlayerTurnPassed.Invoke());
        _tapButton.onClick.AddListener(() => { CardSystem.Instance.Tap(); _tapUsed = true; _tapButton.interactable = false; });

        CardSystem.Instance.OnDeckSizeChanged.AddListener((newValue) => {
            if (newValue == 0)
                _tapButton.interactable = false;
            else
                _tapButton.interactable = true;
        });

        GameEventSystem.Instance.OnBeginTurn.AddListener(() => _tapUsed = false); ;
    }
}
