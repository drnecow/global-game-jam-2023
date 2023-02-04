using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem Instance { get; private set; } // GameEventSystem is a Singleton

    public UnityEvent OnPlayerTurnPassed;
    public UnityEvent OnEndTurn;

    public UnityEvent<int> OnTurnTimerValueChanged;
    public UnityEvent<RootBlock, int> OnRootLifeTimerSet;
    public UnityEvent<RootBlock, int> OnRootLifeTimerChanged;
    public UnityEvent<RootBlock> OnRootLifeTimerExpired;

    public UnityEvent<RootBlock> OnRootBlockDestroyed;

    public UnityEvent OnWinGame;
    public UnityEvent OnLoseGame;

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
