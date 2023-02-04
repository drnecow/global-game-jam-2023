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

    private bool _isBeingEaten = false;
    private Medvedka _eater = null;

    public bool IsImmersedInWater { get => _isImmersedInWater; set => _isImmersedInWater = value; }
    public bool IsFireProof { get => _isFireProof; set => _isFireProof = value; }
    public bool IsHardened { get => _isHardened; set => _isHardened = value; }

    public bool IsBurning { get => _isBurning; set => _isBurning = value; }
    public bool IsBeingEaten { get => _isBeingEaten; set => _isBeingEaten = value; }
    public Medvedka Eater { get => _eater; set => _eater = value; }
    public Root ParentRoot { get => _parentRoot; }

    public void SetOnFire()
    {
        _isBurning = true;

        if (_lifeTimer == 0)
            SetLifeTimer(4);

        if (_isBeingEaten) {
            _eater.DestroyThis();
            _isBeingEaten = false;
        }

        Debug.Log($"{this} set on fire");
    }
    public void BeginBeingEaten(Medvedka eater)
    {
        _isBeingEaten = true;
        _eater = eater;

        SetLifeTimer(eater.EatingTimer);

        Debug.Log($"{this} began being eaten by {eater}");
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
       
        if (_lifeTimer == 0)
        {
            Debug.Log($"Life timer of {this} reached 0, it will be destroyed this turn");
            GameController.Instance.AddTimedOutRootBlock(this);

            if (_eater != null)
            {
                GameController.Instance.AddMedvedka(_eater);
                Map.Instance.SetExistingObjectCoords(_eater);
                _eater.GetComponent<Animator>().SetBool("Eating", false);
            }
        }
        else
        {
            GameEventSystem.Instance.OnRootLifeTimerChanged.Invoke(this, _lifeTimer);
            Debug.Log($"Life timer of {this} set to {_lifeTimer}");
        }
    }
}
