using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlAnimation : MonoBehaviour
{
    [SerializeField] private float _swirlSpeed;
    private float _currentAngle = 0f;

    void Update()
    {
        _currentAngle += Time.deltaTime * _swirlSpeed;
        _currentAngle = Mathf.Repeat(_currentAngle, 360);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -_currentAngle);
    }
}
