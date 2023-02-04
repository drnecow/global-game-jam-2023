using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTimersController : MonoBehaviour
{
    public static RootTimersController Instance { get; private set; }

    [SerializeField] GameObject _timerPrefab;
    Dictionary<RootBlock, RootTimer> _timers = new Dictionary<RootBlock, RootTimer>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("RootTimersController instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;
    }
    private void Start()
    {
        GameEventSystem.Instance.OnRootLifeTimerSet.AddListener((rootBlock, value) => SetTimerUI(rootBlock, value));
        GameEventSystem.Instance.OnRootLifeTimerChanged.AddListener((rootBlock, newValue) => _timers[rootBlock].UpdateTimerValue(newValue));
        GameEventSystem.Instance.OnRootLifeTimerExpired.AddListener((rootBlock) => _timers.Remove(rootBlock));
    }

    private void SetTimerUI(RootBlock rootBlock, int value)
    {
        GameObject timerObj = Instantiate(_timerPrefab);
        timerObj.GetComponent<Canvas>().worldCamera = Camera.main;

        timerObj.transform.SetParent(rootBlock.transform);
        timerObj.transform.localPosition = Vector3.zero;

        RootTimer timer = timerObj.GetComponent<RootTimer>();

        _timers.Add(rootBlock, timer);
        timer.UpdateTimerValue(value);
    }

    public void RemoveRootBlock(RootBlock rootBlock)
    {
        if (_timers.ContainsKey(rootBlock))
        {
            Debug.Log($"Removing {rootBlock}");
            _timers.Remove(rootBlock);
        }
        else
            Debug.Log($"No root block {rootBlock}");
    }
}
