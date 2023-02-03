using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;
using CodeMonkey.Utils;

public class RootBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _singleRoot;
    [SerializeField] private GameObject _lineRoot;
    [SerializeField] private GameObject _zigzagRoot;
    [SerializeField] private GameObject _outgrowthLineRoot;
    [SerializeField] private GameObject _cornerRoot;
    [SerializeField] private GameObject _crossRoot;

    private Vector3 _mousePos;
    private bool _placementFinished = true;


    private void Start()
    {
        StartCoroutine(PlaceRoot(RootType.Line));
    }

    public IEnumerator PlaceRoot(RootType rootType)
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
        // Записать позицию мыши
        _mousePos = UtilsClass.GetMouseWorldPosition();
        // Притянуть к позиции мыши
        newRoot.transform.position = _mousePos;

        while (!_placementFinished)
        {
            Vector3 newMousePos = UtilsClass.GetMouseWorldPosition();

            // Если координаты мыши изменились...
            if (newMousePos != _mousePos) {
                _mousePos = newMousePos;
                Highlight.Instance.ClearHighlight();

                // ...Конвертировать их в координаты карты
                Coords newCoords = Map.Instance.WorldPosToXY(newMousePos);

                // Eсли валидные — переместить объект на них
                if (Map.Instance.ValidateCoords(newCoords))
                {
                    newRoot.transform.position = Map.Instance.XYToWorldPos(newCoords);
                    List<Coords> rootCoords = newRootObj.GetAllCoords();

                    // Подсветить объект красным, если: 1) не весь влезает на карту; 2) накладывается на непроходимую местность; 3) полностью накладывается на существующие корни
                    if (!CanPlace(newRootObj, rootCoords))
                        Highlight.Instance.MarkDenied(rootCoords);
                    // В остальных случаях подсветить зелёным
                    else
                        Highlight.Instance.MarkAllowed(rootCoords);                 
                }
            }

            // WASD/стрелки...
                // Изменить вращение объекта: W — 0, A — 270, S — 180, D — 90

            // Левая кнопка мыши...
            if (Input.GetMouseButton(0))
            {
                List<Coords> rootCoords = newRootObj.GetAllCoords();

                // Проверить, валидна ли позиция объекта (те же критерии, что для подсветки красным/зелёным)
                if (CanPlace(newRootObj, rootCoords))
                {
                // Если да:
                    // Если объект помещается на полностью не занятое существующими корнями пространство:
                    if (RootMap.Instance.IsEmpty(rootCoords))
                    {
                        // Дать ему позицию соответствующей клетки
                        // Получить все координаты объекта
                        // Запустить процессы всех интерактивных объектов на этих координатах
                        // Зарегистрировать объект на основной карте
                        // Получить все подобъекты по координатам объекта
                        // Зарегистрировать компонент RootBlock каждого подобъекта на карте корней
                        // Размещение окончено
                    }
                    // Если объект частично накладывается на существующие корни:
                        // Получить список свободных координат, на которые накладывается объект
                        // Создать корнеблоки-единички по их количеству
                        // Поместить корнеблоки-единички на каждую координату
                        // Запустить процессы всех интерактивных объектов на этих координатах
                        // Зарегистрировать единички на основной карте
                        // Зарегистрировать единички на карте корней
                        //Размещение окончено
                }
                // Если нет:
                    // Debug.Log об этом
            }

            yield return null;
        }
    }

    private bool CanPlace(Root root, List<Coords> rootCoords)
    {
        bool outOfMap = false;
        bool impassableTerrain = false;
        bool overlapsAllRoots = true;

        List<MapObject> overlapObjects = new List<MapObject>();

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

        foreach (MapObject obj in overlapObjects)
        {
            if (!root.IsDrilling && Constants.DESTRUCTIBLE.Contains(obj.EntityType) ||
                obj.EntityType == EntityType.Tree)
                impassableTerrain = true;
        }

        foreach (Coords coord in rootCoords)
        {
            if (Map.Instance.ValidateCoords(coord))
                if (RootMap.Instance.Roots[coord.x, coord.y] == null)
                    overlapsAllRoots = false;
        }

        Debug.Log($"Out of map: {outOfMap}");
        Debug.Log($"Impassable terrain: {impassableTerrain}");
        Debug.Log($"Overlaps all roots: {overlapsAllRoots}");

        if (outOfMap || impassableTerrain || overlapsAllRoots)
            return false;
        else
            return true;
    }
}
