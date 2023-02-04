using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;
using CodeMonkey.Utils;

public class RootBuilder : MonoBehaviour
{
    public static RootBuilder Instance { get; private set; }

    [SerializeField] private GameObject _singleRoot;
    [SerializeField] private GameObject _lineRoot;
    [SerializeField] private GameObject _zigzagRoot;
    [SerializeField] private GameObject _outgrowthLineRoot;
    [SerializeField] private GameObject _cornerRoot;
    [SerializeField] private GameObject _crossRoot;

    private Vector3 _mousePos;
    private bool _placementFinished = true;

    private bool _fireProof;
    private bool _hardened;
    private bool _drilling;

    [SerializeField] private RootType _rootTypeToPlace;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("RootBuilder instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;
    }

    public void PlaceChosenRoot()
    {
        StartCoroutine(HandleRootPlacement(_rootTypeToPlace));
    }
    public void PlaceRoot(RootType rootType)
    {
        StartCoroutine(HandleRootPlacement(rootType));
    }
    public IEnumerator HandleRootPlacement(RootType rootType)
    {
        GameObject rootPrefab = rootType switch
        {
            RootType.Single => _singleRoot,
            RootType.Line => _lineRoot,
            RootType.Zigzag => _zigzagRoot,
            RootType.OutgrowthLine => _outgrowthLineRoot,
            RootType.Corner => _cornerRoot,
            RootType.Cross => _crossRoot,
            _ => null
        };

        if (rootPrefab == null)
        {
            Debug.Log($"No suitable root block prefab for type {rootType}");
            yield break;
        }

        _placementFinished = false;

        // Создать копию префаба корнеблока, т.е. объект
        GameObject newRoot = Instantiate(rootPrefab);
        Root newRootObj = newRoot.GetComponent<Root>();

        // Проверить модификаторы корня и сделать огнеупорным / сделать прочным, сбросить соответствующие модификаторы
        if (_fireProof)
        {
            newRootObj.MakeFireProof();
            _fireProof = false;
        }
        else if (_hardened)
        {
            newRootObj.MakeHardened();
            _hardened = false;
        }

        // Записать позицию мыши
        _mousePos = UtilsClass.GetMouseWorldPosition();
        // Притянуть к позиции мыши
        newRoot.transform.position = _mousePos;

        while (!_placementFinished)
        {
            Vector3 newMousePos = UtilsClass.GetMouseWorldPosition();

            // Если координаты мыши изменились...
            HandleMousePositionChange(newMousePos, newRootObj);

            // WASD/стрелки...
                // Изменить вращение объекта: W — 0, A — 270, S — 180, D — 90
            HandleInputRotation(newRootObj);

            // Левая кнопка мыши...
            HandleLeftClick(newRootObj);

            yield return null;
        }
    }

    private void HandleMousePositionChange(Vector3 newMousePos, Root root)
    {
        if (newMousePos != _mousePos)
        {
            _mousePos = newMousePos;
            Highlight.Instance.ClearHighlight();

            // ...Конвертировать их в координаты карты
            Coords newCoords = Map.Instance.WorldPosToXY(newMousePos);

            // Eсли валидные — переместить объект на них
            if (Map.Instance.ValidateCoords(newCoords))
            {
                root.transform.position = Map.Instance.XYToWorldPos(newCoords);
                List<Coords> rootCoords = root.GetAllCoords();

                // Подсветить объект красным, если: 1) не весь влезает на карту; 2) накладывается на непроходимую местность; 3) полностью накладывается на существующие корни
                if (!CanPlace(root))
                    Highlight.Instance.MarkDenied(rootCoords);
                // В остальных случаях подсветить зелёным
                else
                    Highlight.Instance.MarkAllowed(rootCoords);
            }
        }
    }
    private void HandleInputRotation(Root root)
    {
        float rotation = root.transform.eulerAngles.z;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            rotation = 0f;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            rotation = 270f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            rotation = 90f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            rotation = 180f;

        root.transform.eulerAngles = new Vector3(0f, 0f, rotation);

        Highlight.Instance.ClearHighlight();

        if (CanPlace(root))
            Highlight.Instance.MarkAllowed(root.GetAllCoords());
        else
            Highlight.Instance.MarkDenied(root.GetAllCoords());
    }
    private void HandleLeftClick(Root root)
    {
        if (Input.GetMouseButton(0))
        {
            _mousePos = UtilsClass.GetMouseWorldPosition();
            Coords mouseCoords = Map.Instance.WorldPosToXY(_mousePos);

            // Если координаты мыши валидны, установить объект на них
            if (Map.Instance.ValidateCoords(mouseCoords))
            {
                root.transform.position = Map.Instance.XYToWorldPos(mouseCoords);
                List<Coords> rootCoords = root.GetAllCoords();

                if (CanPlace(root))
                {
                    if (!RootMap.Instance.IsEmpty(rootCoords))
                    {
                        List<Coords> nonEmptyCoords = RootMap.Instance.GetNonEmpty(rootCoords);

                        foreach (Coords coord in nonEmptyCoords)
                            root.DestroyObjectAt(coord);

                        rootCoords = root.GetAllCoords();
                    }

                    Interact(root, rootCoords);

                    Map.Instance.SetExistingObjectCoords(root);

                    foreach (Coords coord in rootCoords)
                    {
                        RootBlock rootBlock = root.GetObjectAt(coord).GetComponent<RootBlock>();
                        RootMap.Instance.AddRootBlock(coord, rootBlock);
                    }

                    RemoveFogOfWar(rootCoords);
                    Highlight.Instance.ClearHighlight();

                    _placementFinished = true;
                }
            }
            else
            {
                Debug.LogWarning($"Invalid placement position: ({mouseCoords.x}, {mouseCoords.y})");
            }
        }
    }

    // Determines if a root block can be placed on given coordinates of the map
    private bool CanPlace(Root root)
    {
        List<Coords> rootCoords = root.GetAllCoords();

        bool outOfMap = false;
        bool impassableTerrain = false;
        bool overlapsAllRoots = true;
        bool connected = false;

        List<MapObject> overlapObjects = new List<MapObject>();

        // Is out of map?
        foreach (Coords coord in rootCoords)
        {
            if (Map.Instance.ValidateCoords(coord))
            {
                if (Map.Instance.GridArray[coord.x, coord.y] != null)
                    overlapObjects.Add(Map.Instance.GridArray[coord.x, coord.y]);
            }
            else
                outOfMap = true;
        }
        // Is overlapping any impassable terrain?
        foreach (MapObject obj in overlapObjects)
        {
            if (!_drilling && Constants.DESTRUCTIBLE.Contains(obj.EntityType) ||
                obj.EntityType == EntityType.Tree)
                impassableTerrain = true;
        }
        // Is overlapping all previously placed roots?
        foreach (Coords coord in rootCoords)
        {
            if (Map.Instance.ValidateCoords(coord))
                if (RootMap.Instance.Roots[coord.x, coord.y] == null)
                    overlapsAllRoots = false;
        }
        // Is connected to any previously placed roots or to points of origin?
        List<Coords> rootBlocks = root.GetAllCoords();

        foreach(Coords block in rootBlocks)
        {
            List<Coords> neighbours = Map.Instance.GetNeighbours(block, Constants.NEIGHBOURS_1X1);

            foreach (Coords neighbour in neighbours)
            {
                MapObject neighbourObj = Map.Instance.GridArray[neighbour.x, neighbour.y];

                if (neighbourObj != null && neighbourObj.EntityType == EntityType.Root
                    || Constants.ORIGIN_POINTS.Contains(neighbour))
                    connected = true;
            }
        }

        //Debug.Log($"Out of map: {outOfMap}");
        //Debug.Log($"Impassable terrain: {impassableTerrain}");
        //Debug.Log($"Overlaps all roots: {overlapsAllRoots}");
        //Debug.Log($"Connected to previous roots: {connected}");

        if (outOfMap || impassableTerrain || overlapsAllRoots || !connected)
            return false;
        else
            return true;
    }
    // Interact with all objects on given coordinates with root rootObj
    private void Interact(Root root, List<Coords> coords)
    {
        List<MapObject> interacted = new List<MapObject>();
        Dictionary<MapObject, List<RootBlock>> burnTargets = new Dictionary<MapObject, List<RootBlock>>();

        // Запустить процессы всех интерактивных объектов на этих координатах
        // Если это корень-бур, уничтожить объекты на его координатах и сбросить корень-бур
        foreach (Coords coord in coords)
        {
            MapObject obj = Map.Instance.GridArray[coord.x, coord.y];
            
            if (obj != null)
            {
                if (_drilling && Constants.DESTRUCTIBLE.Contains(obj.EntityType))
                {
                    if (obj.EntityType == EntityType.Rock)
                        obj.DestroyObjectAt(coord);
                    else
                        obj.DestroyThis();
                }
                else if (Constants.INTERACTABLE_ONCE.Contains(obj.EntityType) && !interacted.Contains(obj))
                {
                    obj.RunRootInteractionProcess();
                    interacted.Add(obj);
                }
                else if (obj.EntityType == EntityType.Water)
                {
                    RootBlock rootBlock = root.GetObjectAt(coord).GetComponent<RootBlock>();
                    obj.RunRootInteractionProcess(rootBlock);
                }
                else if (obj.EntityType == EntityType.Fire)
                {
                    if (!burnTargets.ContainsKey(obj))
                        burnTargets.Add(obj, new List<RootBlock> { root.GetObjectAt(coord).GetComponent<RootBlock>() });
                    else
                        burnTargets[obj].Add(root.GetObjectAt(coord).GetComponent<RootBlock>());
                }
            }
        }

        foreach (MapObject fire in burnTargets.Keys)
        {
            RootBlock[] roots = burnTargets[fire].ToArray();
            fire.RunRootInteractionProcess(roots);
        }

        if (_drilling)
            _drilling = false;
    }
    // Remove 3 squares of fog of war in all directions around specified root blocks
    private void RemoveFogOfWar(List<Coords> coords)
    {
        foreach (Coords coord in coords)
        {
            // Each coord's neighbours in 3 cells around
            List<Coords> neighbours3 = new List<Coords>();

            for (int i = -3; i < 4; i++)
                for (int j = -3; j < 4; j++)
                    neighbours3.Add(new Coords(coord.x + i, coord.y + j));

            FogOfWar.Instance.RemoveSquaresAt(neighbours3);
        }
    }
}
