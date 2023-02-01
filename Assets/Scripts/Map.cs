using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;
using CodeMonkey.Utils;

// Simple struct that stores grid cell's coordinates
public struct Coords
{
    public int x;
    public int y;

    public Coords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static bool IsEqual(Coords a, Coords b)
    {
        if (a.x == b.x && a.y == b.y)
            return true;
        else
            return false;
    }
}

public class Map : MonoBehaviour
{
    public static Map Instance { get; private set; } // Map is a Singleton

    [SerializeField] private int _width;
    [SerializeField] private int _height;
    private float _cellSize = Constants.MAP_CELL_SIZE;
    [SerializeField] private Vector3 _originPosition; // point in the world from which the map is drawn, left to right and top to bottom

    private TextMesh[,] _textArray;

    [SerializeField] bool debugView;
    

    void Awake()
    {
        // Setup Singleton behavior
        if (Instance != null && Instance != this)
        {
            Debug.Log("Map instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;


        _textArray = new TextMesh[_width, _height];

        // Draw vertical grid lines
        for (int i = 0; i <= _width; i++)
            Debug.DrawLine(new Vector3(i * _cellSize, 0) + _originPosition, new Vector3(i * _cellSize, -(_height * _cellSize)) + _originPosition, Color.blue, 10000f);
        // Draw horizontal grid lines
        for (int j = 0; j <= _height; j++)
            Debug.DrawLine(new Vector3(0, -(j * _cellSize)) + _originPosition, new Vector3(_width * _cellSize, -(j * _cellSize)) + _originPosition, Color.blue, 10000f);

        if (debugView)
            for (int x = 0; x < _textArray.GetLength(0); x++)
                for (int y = 0; y < _textArray.GetLength(1); y++)
                    _textArray[x, y] = UtilsClass.CreateWorldText($"({x}, {y})", null, XYToWorldPos(x, y), 20, Color.blue, TextAnchor.MiddleCenter);
    }

    public Vector3 XYToWorldPos(int x, int y)
    {
        return new Vector3(x * _cellSize + _cellSize / 2, -(y * _cellSize + _cellSize / 2)) + _originPosition;
    }
    public Coords WorldPosToXY(Vector3 worldPosition)
    {
        Coords coords = new Coords
        {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize),
            y = Mathf.FloorToInt(-(worldPosition - _originPosition).y / _cellSize)
        };

        return coords;
    }

    public bool ValidateCoords(int x, int y)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
            return true;
        else
            return false;
    }
}