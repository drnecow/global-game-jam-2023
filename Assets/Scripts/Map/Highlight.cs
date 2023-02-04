using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public static Highlight Instance { get; private set; } // Highlight is a Singleton

    private GameObject[,] _squares;
    private SpriteRenderer[,] _spriteRenderers;
    [SerializeField] private GameObject _squarePrefab;

    private float _alpha = 0.2f;


    private void Awake()
    {
        // Setup Singleton behavior
        if (Instance != null && Instance != this)
        {
            Debug.Log("Highlight instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;
    }

    private void Start()
    {
        _squares = new GameObject[Map.Instance.Width, Map.Instance.Height];
        _spriteRenderers = new SpriteRenderer[Map.Instance.Width, Map.Instance.Height];

        SetupSquares();
    }

    private void SetupSquares()
    {
        for (int i = 0; i < _squares.GetLength(0); i++)
            for (int j = 0; j < _squares.GetLength(1); j++)
            {
                GameObject newSquare = Instantiate(_squarePrefab);
                newSquare.transform.position = Map.Instance.XYToWorldPos(new Coords(i, j));
                newSquare.transform.SetParent(transform);

                _squares[i, j] = newSquare;
                _spriteRenderers[i, j] = newSquare.GetComponent<SpriteRenderer>();
            }
    }

    public void MarkDenied(List<Coords> coords)
    {
        foreach (Coords coord in coords)
        {
            if (Map.Instance.ValidateCoords(coord))
            {
                Color color = _spriteRenderers[coord.x, coord.y].color;

                color.r = 255f;
                color.g = 0f;
                color.b = 0f;
                color.a = _alpha;

                _spriteRenderers[coord.x, coord.y].color = color;
            }
        }
    }
    public void MarkDenied(Coords coords)
    {
        if (Map.Instance.ValidateCoords(coords))
        {
            Color color = _spriteRenderers[coords.x, coords.y].color;

            color.r = 255f;
            color.g = 0f;
            color.b = 0f;
            color.a = _alpha;

            _spriteRenderers[coords.x, coords.y].color = color;
        }
    }
    public void MarkAllowed(List<Coords> coords)
    {
        foreach (Coords coord in coords)
        {
            if (Map.Instance.ValidateCoords(coord))
            {
                Color color = _spriteRenderers[coord.x, coord.y].color;

                color.r = 0f;
                color.g = 255f;
                color.b = 0f;
                color.a = _alpha;

                _spriteRenderers[coord.x, coord.y].color = color;
            }
        }
    }
    public void MarkAllowed(Coords coords)
    {
        if (Map.Instance.ValidateCoords(coords))
        {
            Color color = _spriteRenderers[coords.x, coords.y].color;

            color.r = 0f;
            color.g = 255f;
            color.b = 0f;
            color.a = _alpha;

            _spriteRenderers[coords.x, coords.y].color = color;
        }
    }
    public void ClearHighlight()
    {
        foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            Color color = spriteRenderer.color;
            color.a = 0f;
            spriteRenderer.color = color;
        }
    }
}
