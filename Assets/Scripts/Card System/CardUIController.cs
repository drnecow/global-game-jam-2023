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
    [SerializeField] private TextMeshProUGUI _tapButtonText;
    [SerializeField] private Image _tapButtonIcon;


    private void Start()
    {
        CardSystem.Instance.OnDeckSizeChanged.AddListener((newValue) => _deckCardNumber.text = newValue.ToString());
        CardSystem.Instance.OnDiscardSizeChanged.AddListener((newValue) => _discardCardNumber.text = newValue.ToString());

        _endTurnButton.onClick.AddListener(() => GameEventSystem.Instance.OnPlayerTurnPassed.Invoke());
        _tapButton.onClick.AddListener(() => { CardSystem.Instance.Tap();
            _tapButton.interactable = false;
            _tapButtonText.color = new Color(0.3070132f, 0.4168667f, 0.4622642f, 0.5490196f);
            _tapButtonIcon.color = new Color(0.5f, 0.5f, 0.5f);
        });

        CardSystem.Instance.OnDeckSizeChanged.AddListener((newValue) => {
            if (newValue == 0)
            {
                _tapButton.interactable = false;
                _tapButtonText.color = new Color(0f, 0f, 0f);
                _tapButtonIcon.color = new Color(0.5f, 0.5f, 0.5f);
            }
            else
            {
                _tapButton.interactable = true;
                _tapButtonText.color = new Color(0.4377358f, 0.868969f, 1f);
                _tapButtonIcon.color = new Color(1f, 1f, 1f, 1f);
            }
        });

    }
}
