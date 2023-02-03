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

        // ������� ����� ������� ����������, �.�. ������
        GameObject newRoot = Instantiate(rootPrefab);
        Root newRootObj = newRoot.GetComponent<Root>();
        // �������� ������� ����
        _mousePos = UtilsClass.GetMouseWorldPosition();
        // ��������� � ������� ����
        newRoot.transform.position = _mousePos;

        while (!_placementFinished)
        {
            Vector3 newMousePos = UtilsClass.GetMouseWorldPosition();

            // ���� ���������� ���� ����������...
            if (newMousePos != _mousePos) {
                _mousePos = newMousePos;
                Highlight.Instance.ClearHighlight();

                // ...�������������� �� � ���������� �����
                Coords newCoords = Map.Instance.WorldPosToXY(newMousePos);

                // E��� �������� � ����������� ������ �� ���
                if (Map.Instance.ValidateCoords(newCoords))
                {
                    newRoot.transform.position = Map.Instance.XYToWorldPos(newCoords);
                    List<Coords> rootCoords = newRootObj.GetAllCoords();

                    // ���������� ������ �������, ����: 1) �� ���� ������� �� �����; 2) ������������� �� ������������ ���������; 3) ��������� ������������� �� ������������ �����
                    if (!CanPlace(newRootObj, rootCoords))
                        Highlight.Instance.MarkDenied(rootCoords);
                    // � ��������� ������� ���������� ������
                    else
                        Highlight.Instance.MarkAllowed(rootCoords);                 
                }
            }

            // WASD/�������...
                // �������� �������� �������: W � 0, A � 270, S � 180, D � 90

            // ����� ������ ����...
            if (Input.GetMouseButton(0))
            {
                List<Coords> rootCoords = newRootObj.GetAllCoords();

                // ���������, ������� �� ������� ������� (�� �� ��������, ��� ��� ��������� �������/������)
                if (CanPlace(newRootObj, rootCoords))
                {
                // ���� ��:
                    // ���� ������ ���������� �� ��������� �� ������� ������������� ������� ������������:
                    if (RootMap.Instance.IsEmpty(rootCoords))
                    {
                        // ���� ��� ������� ��������������� ������
                        // �������� ��� ���������� �������
                        // ��������� �������� ���� ������������� �������� �� ���� �����������
                        // ���������������� ������ �� �������� �����
                        // �������� ��� ���������� �� ����������� �������
                        // ���������������� ��������� RootBlock ������� ���������� �� ����� ������
                        // ���������� ��������
                    }
                    // ���� ������ �������� ������������� �� ������������ �����:
                        // �������� ������ ��������� ���������, �� ������� ������������� ������
                        // ������� ����������-�������� �� �� ����������
                        // ��������� ����������-�������� �� ������ ����������
                        // ��������� �������� ���� ������������� �������� �� ���� �����������
                        // ���������������� �������� �� �������� �����
                        // ���������������� �������� �� ����� ������
                        //���������� ��������
                }
                // ���� ���:
                    // Debug.Log �� ����
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
