using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnim : MonoBehaviour
{
    [SerializeField] private List<Sprite> _slides;
    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();   
    }
    void Start()
    {
        StartCoroutine(ShowCutscene());
    }

    private IEnumerator ShowCutscene()
    {
        for (int i = 0; i < 10; i++)
        {
            _spriteRenderer.sprite = _slides[i];
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1f);

        for (int i = 10; i < 20; i++)
        {
            _spriteRenderer.sprite = _slides[i];
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1f);

        for (int i = 20; i < 30; i++)
        {
            _spriteRenderer.sprite = _slides[i];
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);
        Scenes.Instance.LoadGame();
    }
}
