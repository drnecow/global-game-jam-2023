using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    [SerializeField] private GameObject _exitMenu;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _exitMenu.SetActive(true);
        }        
    }
}
