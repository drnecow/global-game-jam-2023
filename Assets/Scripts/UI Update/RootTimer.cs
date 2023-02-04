using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RootTimer : MonoBehaviour
{
    TextMeshProUGUI _timerText;


    private void Awake()
    {
        _timerText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateTimerValue(int newValue)
    {
        _timerText.text = newValue.ToString();
    }
}
