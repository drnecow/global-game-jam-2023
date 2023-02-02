using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Constants;

public class Fire : MapObject
{
    [SerializeField] private List<Coords> _trajectoryPath;
    private int _currentPathCell = 0;
    private bool _isMovingForward = true;


    private void Awake()
    {
        _entityType = EntityType.Fire;
    }

    [ContextMenu("Move Next Cell")]
    public void MoveNextCell() // assumes that _trajectoryPath is at least two items long
    {
        if (_trajectoryPath.Count <= 1)
            return;

        if (_isMovingForward)
        {
            _currentPathCell++;
            if (_currentPathCell == _trajectoryPath.Count - 1)
                _isMovingForward = false;
        }
        else
        {
            _currentPathCell--;
            if (_currentPathCell == 0)
                _isMovingForward = true;
        }

        Map.Instance.ClearExistingObjectCoords(this);

        transform.position = Map.Instance.XYToWorldPos(_trajectoryPath[_currentPathCell]);
        Map.Instance.SetExistingObjectCoords(this);
    }
    private void ResolveContactAt(Coords coords)
    {

    }
    public override void RunRootInteractionProcess(params RootBlock[] rootBlocks)
    {
        base.RunRootInteractionProcess(rootBlocks);
    }
    private void SetRootBlockOnFire(RootBlock rootBlock)
    {

    }
}
