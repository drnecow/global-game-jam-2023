using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryOrDefeatScene : MonoBehaviour
{
    [SerializeField] private List<Sprite> _victorySlides;
    [SerializeField] private List<Sprite> _defeatSlides;

    [SerializeField] private float _slideshowSpeed;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private bool _victory = true;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (Scenes.Instance != null)
        {
            if (Scenes.Instance.Victory)
                ShowVictorySlides();
            else
                ShowDefeatSlides();
        }
        else if (_victory)
            ShowVictorySlides();
        else if (!_victory)
            ShowDefeatSlides();
    }


    public void ShowVictorySlides()
    {
        StartCoroutine(ShowSlides(_victorySlides));
    }
    public void ShowDefeatSlides()
    {
        StartCoroutine(ShowSlides(_defeatSlides));
    }
    private IEnumerator ShowSlides(List<Sprite> slides)
    {
        foreach (Sprite slide in slides)
        {
            _spriteRenderer.sprite = slide;
            yield return new WaitForSeconds(_slideshowSpeed);
        }

        Scenes.Instance.MainMenu();
    }
}
