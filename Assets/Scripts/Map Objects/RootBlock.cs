using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBlock : MonoBehaviour
{
    private int _lifeTimer = 0;
    [SerializeField] private Root _parentRoot;

    private bool _isImmersedInWater = false;
    private bool _isFireProof = false;
    private bool _isHardened = false;

    private bool _isBurning = false;
    //private FireSource _fireSource = null;

    private bool _isBeingEaten = false;
    private Medvedka _eater = null;

    public bool IsImmersedInWater { get => _isImmersedInWater; set => _isImmersedInWater = value; }
    public bool IsFireProof { get => _isFireProof; set => _isFireProof = value; }
    public bool IsHardened { get => _isHardened; set => _isHardened = value; }

    public bool IsBurning { get => _isBurning; set => _isBurning = value; }
    public bool IsBeingEaten { get => _isBeingEaten; set => _isBeingEaten = value; }
    public Medvedka Eater { get => _eater; set => _eater = value; }

    public void SetOnFire()
    {
        _isBurning = true;

        if (_lifeTimer == 0)
            SetLifeTimer(4);

        if (_isBeingEaten) {
            Destroy(_eater);
            _isBeingEaten = false;
        }

        Debug.Log($"{this} set on fire");
    }
    private void SetLifeTimer(int newValue)
    {
        _lifeTimer = newValue;
        GameEventSystem.Instance.OnRootLifeTimerSet.Invoke(this, _lifeTimer);
        GameController.Instance.AddTimeredRootBlock(this);

        Debug.Log($"Life timer of {this} set to {newValue}");
    }
    public void DecrementLifeTimer()
    {
        _lifeTimer--;
        GameEventSystem.Instance.OnRootLifeTimerChanged.Invoke(this, _lifeTimer);
        Debug.Log($"Life timer of {this} set to {_lifeTimer}");

        if (_lifeTimer == 0)
        {
            Debug.Log($"Life timer of {this} reached 0, destroying this");
            _parentRoot.DestroyRootBlockAt(Map.Instance.WorldPosToXY(transform.position));
        }
    }
}
