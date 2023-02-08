using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; } // GameController is a Singleton
    public bool RootBeingPlaced { get => _rootBeingPlaced; set => _rootBeingPlaced = value; }

    private int _turnTimer = 11;

    [SerializeField] List<MedvedkaNest> _medvedkaNests;
    private List<Medvedka> _medvedkasOnMap;
    [SerializeField] private List<Fire> _movingFire;
    private List<FireSource> _fireSources;
    private List<RootBlock> _timeredRootBlocks;

    private List<Medvedka> _eatingMedvedkasToRemove;
    private List<RootBlock> _timedOutRootBlocks;

    private bool _rootBeingPlaced = false;

    private bool _playerInputPassed = false;


    private void Awake()
    {
        // Setup Singleton behavior
        if (Instance != null && Instance != this)
        {
            Debug.Log("GameController instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;

        _medvedkasOnMap = new List<Medvedka>();
        _fireSources = new List<FireSource>();
        _timeredRootBlocks = new List<RootBlock>();

        _timedOutRootBlocks = new List<RootBlock>();
        _eatingMedvedkasToRemove = new List<Medvedka>();
    }

    void Start()
    {
        GameEventSystem.Instance.OnPlayerTurnPassed.AddListener(() => _playerInputPassed = true);
        GameEventSystem.Instance.OnEndTurn.AddListener(() => ResolveTurn());

        GameEventSystem.Instance.OnWinGame.AddListener(() => { Scenes.Instance.Victory = true; Scenes.Instance.VictoryOrDefeat(); });
        GameEventSystem.Instance.OnLoseGame.AddListener(() => { Scenes.Instance.Victory = false; Scenes.Instance.VictoryOrDefeat(); });

        MapSetup.Instance.PopulateMap();
        CardSystem.Instance.StartGame();
        ResolveTurn();
    }

    [ContextMenu("Resolve Turn")]
    public void ResolveTurn()
    {
        StartCoroutine(HandleTurn());   
    }

    private void ResolvePreInputActions()
    {
        CardSystem.Instance.NewTurn();

        if (!DecrementTurnTimer())
            return;

        DecrementRootLifeTimers();
        DestroyTimedOutRoots();

        SpawnMedvedkas();
        MoveMedvedkas();
        RemoveEatingMedvedkas();
        SpreadFireSource();
        MoveFire();
    }
    private void ResolvePostInputActions()
    {
        CardSystem.Instance.EndTurn();
        GameEventSystem.Instance.OnEndTurn.Invoke();
    }
    private IEnumerator HandleTurn()
    {
        ResolvePreInputActions();

        while (!_playerInputPassed)
            yield return null;

        _playerInputPassed = false;
        ResolvePostInputActions();
    }

    public bool DecrementTurnTimer()
    {
        if (_turnTimer > 0)
        {
            _turnTimer--;
            GameEventSystem.Instance.OnTurnTimerValueChanged.Invoke(_turnTimer);
        }

        if (_turnTimer == 0)
        {
            GameEventSystem.Instance.OnLoseGame.Invoke();
            return false;
        }

        return true;
    } 
    public void AddTurnTimer(int bonus)
    {
        _turnTimer += bonus;
        GameEventSystem.Instance.OnTurnTimerValueChanged.Invoke(_turnTimer);
    }

    public void AddTimedOutRootBlock(RootBlock rootBlock)
    {
        if (!_timedOutRootBlocks.Contains(rootBlock))
            _timedOutRootBlocks.Add(rootBlock);
    }

    public void AddFireSource(FireSource fireSource)
    {
        if (!_fireSources.Contains(fireSource))
        {
            _fireSources.Add(fireSource);
            Debug.Log($"Added fire source {fireSource}");
        }
        else
        {
            Debug.Log($"Fire source {fireSource} is already added");
        }
    }
    public void AddTimeredRootBlock(RootBlock rootBlock)
    {
        if (!_timeredRootBlocks.Contains(rootBlock))
        {
            _timeredRootBlocks.Add(rootBlock);
            Debug.Log($"Added timered root block {rootBlock}");
        }
        else
        {
            Debug.Log($"Timered root block {rootBlock} is already added");
        }
    }
    public void AddMedvedka(Medvedka medvedka)
    {
        if (!_medvedkasOnMap.Contains(medvedka))
        {
            _medvedkasOnMap.Add(medvedka);
        }
        else
        {
            Debug.Log($"Medvedka {medvedka} is already added");
        }
    }
    public void AddEatingMedvedkaToRemove(Medvedka medvedka)
    {
        if (!_eatingMedvedkasToRemove.Contains(medvedka))
            _eatingMedvedkasToRemove.Add(medvedka);
    }

    public void RemoveFireSource(FireSource fireSource)
    {
        if (_fireSources.Contains(fireSource))
        {
            Debug.Log($"Removing fire source {fireSource}");
            _fireSources.Remove(fireSource);
            //Destroy(fireSource);
        }
        else
        {
            Debug.Log($"Fire source {fireSource} doesn't exist in the game");
        }
    }
    public void RemoveTimeredRootBlock(RootBlock rootBlock)
    {
        if (_timeredRootBlocks.Contains(rootBlock))
        {
            _timeredRootBlocks.Remove(rootBlock);
            Debug.Log($"Removed root block {rootBlock}");
        }
        else
        {
            Debug.Log($"Root block {rootBlock} is not in the game");
        }
    }
    public void RemoveMedvedka(Medvedka medvedka)
    {
        if (_medvedkasOnMap.Contains(medvedka))
        {
            Debug.Log($"Removing {medvedka}");
            _medvedkasOnMap.Remove(medvedka);
        }
        else
            Debug.Log("No suitable medvedka to be removed");
    }
    public void KillMedvedka(Medvedka medvedka)
    {
        RemoveMedvedka(medvedka);

        RootBlock toRemove = null;

        foreach (RootBlock rootBlock in _timeredRootBlocks)
        {
            if (rootBlock.Eater == medvedka)
            {
                toRemove = rootBlock;
                break;
            }
        }
        _timeredRootBlocks.Remove(toRemove);
    }
    public void RemoveMedvedkaNest(MedvedkaNest medvedkaNest)
    {
        if (_medvedkaNests.Contains(medvedkaNest))
        {
            Debug.Log($"Removing {medvedkaNest}");
            _medvedkaNests.Remove(medvedkaNest);
        }
        else
            Debug.Log("No suitable medvedka nest to be removed");
    }

    private void MoveMedvedkas()
    {
        foreach (Medvedka medvedka in _medvedkasOnMap)
            medvedka.MoveRandomly();
    }
    private void SpawnMedvedkas()
    {
        foreach (MedvedkaNest nest in _medvedkaNests)
            nest.SpawnMedvedka();
    }
    private void MoveFire()
    {
        foreach (Fire fire in _movingFire)
            fire.MoveNextCell();
    }
    private void SpreadFireSource()
    {
        foreach (FireSource fireSource in _fireSources)
            fireSource.Spread();
    }
    private void DecrementRootLifeTimers()
    {
        foreach (RootBlock rootBlock in _timeredRootBlocks)
            rootBlock.DecrementLifeTimer();
    }

    private void DestroyTimedOutRoots()
    {
        foreach (RootBlock root in _timedOutRootBlocks)
        {
            GameEventSystem.Instance.OnRootLifeTimerExpired.Invoke(root);
            GameEventSystem.Instance.OnRootBlockDestroyed.Invoke(root);
            _timeredRootBlocks.Remove(root);

            Coords rootCoords = Map.Instance.WorldPosToXY(root.transform.position);

            root.ParentRoot.DestroyObjectAt(rootCoords);
            RootMap.Instance.RemoveRootBlockAt(rootCoords);
        }

        _timedOutRootBlocks.Clear();

        // Check for detached roots and destroy them as well
    }
    private void RemoveEatingMedvedkas()
    {
        foreach (Medvedka medvedka in _eatingMedvedkasToRemove)
            RemoveMedvedka(medvedka);

        _eatingMedvedkasToRemove.Clear();
    }
}
