using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scenes : MonoBehaviour
{
    public static Scenes Instance { get; private set; }
    public bool Victory { get => _victory; set => _victory = value; }

    private bool _cutsceneWatched;
    private bool _victory;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Scenes instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;
    }

    public void LoadGame()
    {
        DontDestroyOnLoad(this);
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    public void Cutscene()
    {
        DontDestroyOnLoad(this);

        if (!_cutsceneWatched) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            _cutsceneWatched = true;
        }
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    public void MainMenu()
    {
        DontDestroyOnLoad(this);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void VictoryOrDefeat()
    {
        DontDestroyOnLoad(this);
        SceneManager.LoadScene(3);
    }
    public void Quit()
    {
        Application.Quit();   
    }
}
