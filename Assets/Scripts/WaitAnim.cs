using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnim : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(10);
        Scenes.Instance.LoadGame();
    }
}
