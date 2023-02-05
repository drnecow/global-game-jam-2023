using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => Scenes.Instance.MainMenu());
    }
}
