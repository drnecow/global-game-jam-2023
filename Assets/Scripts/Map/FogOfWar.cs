using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public static FogOfWar Instance { get; private set; } // FogOfWar is a Singleton

    private GameObject[,] _squares;
    [SerializeField] private GameObject _squarePrefab;

    [SerializeField] private float _fadeSpeed;

    // Coordinates where AstralSalvations resides
    private List<Coords> _exclusions = new List<Coords>()
    {
        new Coords(32, 6), new Coords(33, 6), new Coords(34, 6),
        new Coords(32, 7), new Coords(33, 7), new Coords(34, 7),
        new Coords(32, 8), new Coords(33, 8), new Coords(34, 8)
    };


    private void Awake()
    {
        // Setup Singleton behavior
        if (Instance != null && Instance != this)
        {
            Debug.Log("FogOfWar instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;
    }

    private void Start()
    {
        _squares = new GameObject[Map.Instance.Width, Map.Instance.Height];
        SetupSquares();
    }

    private void SetupSquares()
    {
        for (int i = 5; i < _squares.GetLength(0); i++)
            for (int j = 0; j < _squares.GetLength(1); j++)
            {
                if (!_exclusions.Contains(new Coords(i, j)))
                {
                    GameObject newSquare = Instantiate(_squarePrefab);
                    newSquare.transform.position = Map.Instance.XYToWorldPos(new Coords(i, j));
                    newSquare.transform.SetParent(transform);

                    _squares[i, j] = newSquare;
                }
            }
    }

    public void RemoveSquaresAt(List<Coords> coords)
    {
        foreach (Coords coord in coords)
            RemoveSquareAt(coord);
    }
    public void RemoveSquareAt(Coords coords)
    {
        if (Map.Instance.ValidateCoords(coords))
            if (_squares[coords.x, coords.y] != null)
                StartCoroutine(DestroySquare(_squares[coords.x, coords.y]));
    }
    private IEnumerator DestroySquare(GameObject square)
    {
        SpriteRenderer spriteRenderer = square.GetComponent<SpriteRenderer>();
        Material squareMat = spriteRenderer.sharedMaterial;
        Material copy = new Material(squareMat);
        spriteRenderer.sharedMaterial = copy;
        squareMat = spriteRenderer.sharedMaterial;

        float fade = 1f;

        while (fade > 0)
        {
            fade -= _fadeSpeed;
            squareMat.SetFloat("_Fade", fade);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(square);
    }
}
