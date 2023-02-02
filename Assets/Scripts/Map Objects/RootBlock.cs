using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBlock : MonoBehaviour
{
    private int _lifeTimer = 0;

    private bool _isImmersedInWater = false;
    private bool _isFireProof = false;
    private bool _isHardened = false;

    private bool _isBurning = false;
    private FireSource _fireSource = null;

    private bool _isBeingEaten = false;
    private Medvedka _eater = null;

    public bool IsImmersedInWater { get => _isImmersedInWater; set => _isImmersedInWater = value; }
    public bool IsFireProof { get => _isFireProof; set => _isFireProof = value; }
    public bool IsHardened { get => _isHardened; set => _isHardened = value; }

    public bool IsBurning { get => _isBurning; set => _isBurning = value; }
    public bool IsBeingEaten { get => _isBeingEaten; set => _isBeingEaten = value; }
}
