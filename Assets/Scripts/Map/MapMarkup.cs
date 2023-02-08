using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMarkup : MonoBehaviour
{
    [SerializeField] private GameObject _horizontalMarkupPrefab;
    [SerializeField] private GameObject _verticalMarkupPrefab;


    void Start()
    {
        GameObject verticalLine1 = Instantiate(_verticalMarkupPrefab);
        verticalLine1.transform.position = Vector3.zero;
        verticalLine1.transform.SetParent(transform);

        GameObject verticalLine2 = Instantiate(_verticalMarkupPrefab);
        verticalLine2.transform.position = new Vector3(Map.Instance.Width * Map.Instance.CellSize, 0, 0);
        verticalLine2.transform.SetParent(transform);

        GameObject horizontalLine1 = Instantiate(_horizontalMarkupPrefab);
        horizontalLine1.transform.position = Vector3.zero;
        horizontalLine1.transform.SetParent(transform);

        GameObject horizontalLine2 = Instantiate(_horizontalMarkupPrefab);
        horizontalLine2.transform.position = new Vector3(0, -(Map.Instance.Height * Map.Instance.CellSize), 0);
        horizontalLine2.transform.SetParent(transform);
    }
}
