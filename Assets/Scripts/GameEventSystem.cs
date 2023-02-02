using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem Instance { get; private set; } // GameEventSystem is a Singleton

    public UnityEvent OnGameWin;
    public UnityEvent OnGameLose;

    public void Awake()
    {
        // Setup Singleton behavior
        if (Instance != null && Instance != this)
        {
            Debug.Log("GameEventSystem instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;
    }
}
