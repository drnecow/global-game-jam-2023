using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;
using CodeMonkey.Utils;

// Simple struct that stores grid cell's coordinates
[System.Serializable]
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
    public int Width { get => _width; }
    public int Height { get => _height; }
    public MapObject[,] GridArray { get => _gridArray; }


    [SerializeField] private int _width;
    [SerializeField] private int _height;
    private float _cellSize = Constants.MAP_CELL_SIZE;
    [SerializeField] private Vector3 _originPosition; // point in the world from which the map is drawn, left to right and top to bottom

    private MapObject[,] _gridArray;
    private TextMesh[,] _textArray;
    private TextMesh[,] _occupyArray;

    private GameObject _markupContainer;
    private GameObject _occupyContainer;

    [SerializeField] bool debugView;
    

    private void Awake()
    {
        // Setup Singleton behavior
        if (Instance != null && Instance != this)
        {
            Debug.Log("Map instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;

        _markupContainer = new GameObject();
        _occupyContainer = new GameObject();

        _gridArray = new MapObject[_width, _height];
        _textArray = new TextMesh[_width, _height];
        _occupyArray = new TextMesh[_width, _height];

        // Draw vertical grid lines
        for (int i = 0; i <= _width; i++)
            Debug.DrawLine(new Vector3(i * _cellSize, 0) + _originPosition, new Vector3(i * _cellSize, -(_height * _cellSize)) + _originPosition, Color.blue, 10000f);
        // Draw horizontal grid lines
        for (int j = 0; j <= _height; j++)
            Debug.DrawLine(new Vector3(0, -(j * _cellSize)) + _originPosition, new Vector3(_width * _cellSize, -(j * _cellSize)) + _originPosition, Color.blue, 10000f);

        if (debugView)
            for (int x = 0; x < _textArray.GetLength(0); x++)
                for (int y = 0; y < _textArray.GetLength(1); y++)
                {
                    _textArray[x, y] = UtilsClass.CreateWorldText($"({x}, {y})", null, XYToWorldPos(new Coords(x, y)), 20, Color.blue, TextAnchor.MiddleCenter);
                    _textArray[x, y].gameObject.transform.SetParent(_markupContainer.transform);
                }
    }

    public Vector3 XYToWorldPos(Coords coords)
    {
        return new Vector3(coords.x * _cellSize + _cellSize / 2, -(coords.y * _cellSize + _cellSize / 2)) + _originPosition;
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
    public bool ValidateCoords(Coords coords)
    {
        if (coords.x >= 0 && coords.x < _width && coords.y >= 0 && coords.y < _height)
            return true;
        else
            return false;
    }

    [ContextMenu("Print Grid")]
    public void PrintGrid()
    {
        for (int i = 0; i < _occupyArray.GetLength(0); i++)
            for (int j = 0; j < _occupyArray.GetLength(1); j++)
                if (_occupyArray[i, j] != null)
                    Destroy(_occupyArray[i, j].gameObject);

        for (int x = 0; x < _gridArray.GetLength(0); x++)
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                if (_gridArray[x, y] == null)
                    _occupyArray[x, y] = UtilsClass.CreateWorldText("/", null, XYToWorldPos(new Coords(x, y)), 50, Color.red, TextAnchor.MiddleCenter);
                else
                    _occupyArray[x, y] = UtilsClass.CreateWorldText("X", null, XYToWorldPos(new Coords(x, y)), 50, Color.green, TextAnchor.MiddleCenter);

                _occupyArray[x, y].transform.SetParent(_occupyContainer.transform);
            }
    }

    public void PlaceObjectAt(Coords coords, MapObject obj)
    {
        if (ValidateCoords(coords))
        {
            obj.gameObject.transform.position = XYToWorldPos(coords);
            List<Coords> objParts = obj.GetAllCoords();

            foreach (Coords part in objParts)
                _gridArray[part.x, part.y] = obj;
        }
        else
            Debug.Log($"Invalid coordinates given: ({coords.x}, {coords.y})");
    }
    public void SetExistingObjectCoords(MapObject obj)
    {
        Coords originCoords = WorldPosToXY(obj.transform.position);

        if (ValidateCoords(originCoords))
        {
            List<Coords> objParts = obj.GetAllCoords();

            foreach (Coords part in objParts)
                _gridArray[part.x, part.y] = obj;
        }
        else
            Debug.Log($"Invalid coordinates of object: ({originCoords.x}, {originCoords.y})");
    }
    public void FreeCoords(Coords coords)
    {
        if (ValidateCoords(coords))
            _gridArray[coords.x, coords.y] = null;
        else
            Debug.Log($"Invalid coordinates given: ({coords.x}, {coords.y})");
    }
    public void ClearExistingObjectCoords(MapObject obj)
    {
        Coords originCoords = WorldPosToXY(obj.transform.position);

        if (ValidateCoords(originCoords))
        {
            List<Coords> objParts = obj.GetAllCoords();

            foreach (Coords part in objParts)
                _gridArray[part.x, part.y] = null;
        }
        else
            Debug.Log($"Invalid coordinates of object: ({originCoords.x}, {originCoords.y})");
    }
}