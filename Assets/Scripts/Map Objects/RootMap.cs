using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class RootMap : MonoBehaviour
{
    public static RootMap Instance { get; private set; } // RootMap is a Singleton
    public RootBlock[,] Roots { get => _roots; }

    private RootBlock[,] _roots;


    private void Awake()
    {
        // Setup Singleton behavior
        if (Instance != null && Instance != this)
        {
            Debug.Log("RootMap instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;
    }
    private void Start()
    {
        _roots = new RootBlock[Map.Instance.Width, Map.Instance.Height];
        //Debug.Log($"Root map: width — {_roots.GetLength(0)}, height — {_roots.GetLength(1)}");
    }

    public void AddRootBlock(Coords coords, RootBlock rootBlock)
    {
        if (Map.Instance.ValidateCoords(coords))
        {
            _roots[coords.x, coords.y] = rootBlock;
            //Debug.Log($"Root block {rootBlock} added at coordinates ({coords.x}, {coords.y})");
        }
        else
            Debug.Log($"Invalid coordinates given: ({coords.x}, {coords.y})");
    }
    public void RemoveRootBlockAt(Coords coords)
    {
        if (Map.Instance.ValidateCoords(coords))
        {
            _roots[coords.x, coords.y] = null;
            Debug.Log($"Root block removed from coordinates ({coords.x}, {coords.y})");
        }
    }

    public void EmptyCoords(Coords coords)
    {
        if (Map.Instance.ValidateCoords(coords)) {
            RootBlock removedRoot = _roots[coords.x, coords.y];

            _roots[coords.x, coords.y] = null;
            removedRoot.ParentRoot.DestroyObjectAt(coords);
        }
    }

    public bool IsEmpty(List<Coords> coords)
    {
        if (coords.Count > 0)
        {
            foreach (Coords coord in coords)
            {
                if (Map.Instance.ValidateCoords(coord) && _roots[coord.x, coord.y] != null)
                    return false;
            }

            return true;
        }

        return true;
    }
    public List<Coords> GetEmpty(List<Coords> coords)
    {
        List<Coords> emptyCoords = new List<Coords>();

        foreach (Coords coord in coords)
            if (Map.Instance.ValidateCoords(coord))
                if (_roots[coord.x, coord.y] == null)
                    emptyCoords.Add(coord);

        return emptyCoords;
    }
    public List<Coords> GetNonEmpty(List<Coords> coords)
    {
        List<Coords> nonEmptyCoords = new List<Coords>();

        foreach (Coords coord in coords)
            if (Map.Instance.ValidateCoords(coord))
                if (_roots[coord.x, coord.y] != null)
                    nonEmptyCoords.Add(coord);

        return nonEmptyCoords;
    }
    
    public List<Coords> FindAllDetachedRoots()
    {
        List<Coords> totalIsolatedNodes = new List<Coords>();

        Debug.Log("Finding all isolated roots");

        List<Coords> isolated1 = FindDetachedRoots(Constants.ORIGIN_POINTS[0]);
        List<Coords> isolated2 = FindDetachedRoots(Constants.ORIGIN_POINTS[1]);
        List<Coords> isolated3 = FindDetachedRoots(Constants.ORIGIN_POINTS[2]);

        foreach (Coords node in isolated1)
            if (isolated2.Contains(node) && isolated3.Contains(node))
                totalIsolatedNodes.Add(node);

        foreach (Coords node in isolated2)
            if (isolated1.Contains(node) && isolated3.Contains(node) && !totalIsolatedNodes.Contains(node))
                totalIsolatedNodes.Add(node);

        foreach (Coords node in isolated3)
            if (isolated1.Contains(node) && isolated2.Contains(node) && !totalIsolatedNodes.Contains(node))
                totalIsolatedNodes.Add(node);

        return totalIsolatedNodes;
    }
    public List<Coords> FindDetachedRoots(Coords origin)
    {
        Coords startNode = origin;

        Queue<Coords> frontier = new Queue<Coords>();
        frontier.Enqueue(startNode);

        Dictionary<Coords, Coords> cameFrom = new Dictionary<Coords, Coords>
        {
            { startNode, new Coords(-1000, -1000) }
        };

        while (frontier.Count > 0)
        {
            Coords currentNode = frontier.Dequeue();
            List<Coords> neighbours = GetNeighbours(currentNode, Constants.NEIGHBOURS_1X1);

            foreach (Coords neighbour in neighbours)
            {
                if (!cameFrom.ContainsKey(neighbour))
                {
                    frontier.Enqueue(neighbour);
                    cameFrom.Add(neighbour, currentNode);
                }
            }
        }

        List<Coords> isolated = new List<Coords>();

        for (int i = 0; i < _roots.GetLength(0); i++)
            for (int j = 0; j < _roots.GetLength(1); j++)
            {
                Coords currentNode = new Coords(i, j);
                
                if (_roots[i, j] != null && !cameFrom.ContainsKey(currentNode))
                    isolated.Add(currentNode);
            }

        return isolated;
    }

    public List<Coords> GetNeighbours(Coords coords, List<Vector2> displVectors)
    {
        List<Coords> neighbours = new List<Coords>();

        foreach (Vector2 dir in displVectors)
        {
            Coords neighbourCoords = new Coords(coords.x + (int)dir.x, coords.y + (int)dir.y);

            if (Map.Instance.ValidateCoords(neighbourCoords) && _roots[neighbourCoords.x, neighbourCoords.y] != null)
                neighbours.Add(neighbourCoords);
        }

        return neighbours;
    }
    public void PoisonAllEatingMedvedkas()
    {
        for (int i = 0; i < _roots.GetLength(0); i++)
            for (int j = 0; j < _roots.GetLength(1); j++)
            {
                RootBlock rootBlock = _roots[i, j];

                if (rootBlock != null)
                    if (rootBlock.Eater != null)
                    {
                        rootBlock.Eater.DestroyThis();
                        rootBlock.IsBeingEaten = false;
                        rootBlock.ResetLifeTimer();
                    }
            }
    }
}
