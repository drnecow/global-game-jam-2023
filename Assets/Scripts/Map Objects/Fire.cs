using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void SetNewTrajectory(List<Coords> trajectory)
    {
        _trajectoryPath = trajectory;
        _currentPathCell = 0;
        _isMovingForward = true;
    }

    [ContextMenu("Move Next Cell")]
    public void MoveNextCell() 
    {
        if (_trajectoryPath.Count <= 1)
            return;

        if (_isMovingForward)
        {
            _currentPathCell = Mathf.Min(_currentPathCell + 2, _trajectoryPath.Count - 1);
            if (_currentPathCell == _trajectoryPath.Count - 1)
                _isMovingForward = false;
        }
        else
        {
            _currentPathCell = Mathf.Max(_currentPathCell - 2, 0); ;
            if (_currentPathCell == 0)
                _isMovingForward = true;
        }

        Map.Instance.ClearExistingObjectCoords(this);

        CoroutineAnimation.Instance.MoveTo(gameObject, Map.Instance.XYToWorldPos(_trajectoryPath[_currentPathCell]));
        //transform.position = Map.Instance.XYToWorldPos(_trajectoryPath[_currentPathCell]);

        // Determine objects with which the fire interacted and send them to resolve interactions
        //List<Coords> newCoords = GetAllCoords();
        List<Coords> newCoords = GetAllCoordsAssumingCenter(_trajectoryPath[_currentPathCell]);
        Dictionary<Coords, MapObject> interactObjects = new Dictionary<Coords, MapObject>();

        foreach (Coords coord in newCoords)
            if (Map.Instance.GridArray[coord.x, coord.y] != null)
                interactObjects.Add(coord, Map.Instance.GridArray[coord.x, coord.y]);

        if (interactObjects.Count > 0)
        {
            Dictionary<Coords, MapObject> roots = interactObjects
                .Where(entry => entry.Value.EntityType == EntityType.Root)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            Dictionary<Coords, MapObject> withoutRoots = interactObjects
                .GroupBy(pair => pair.Value)
                .Where(entry => entry.Key.EntityType != EntityType.Root)
                .Select(group => group.First())
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            interactObjects.Clear();

            foreach (KeyValuePair<Coords, MapObject> root in roots)
                interactObjects.Add(root.Key, root.Value);

            foreach (KeyValuePair<Coords, MapObject> noRoot in withoutRoots)
                interactObjects.Add(noRoot.Key, noRoot.Value);

            ResolveContactAt(interactObjects);
        }

        //Map.Instance.SetExistingObjectCoords(this);
        Map.Instance.SetOccupied(_trajectoryPath[_currentPathCell], this);
    }

    private void ResolveContactAt(Dictionary<Coords, MapObject> interactObjects)
    {
        List<RootBlock> flammableRoots = new List<RootBlock>();

        foreach (Coords key in interactObjects.Keys)
        {
            Debug.Log($"Entity type: {interactObjects[key].EntityType}");

            if (interactObjects[key].EntityType == EntityType.Root)
            {
                Debug.Log("Root detected");
                RootBlock rootBlock = interactObjects[key].GetObjectAt(key).GetComponent<RootBlock>();
                Debug.Log(interactObjects[key].GetObjectAt(key));

                if (!rootBlock.IsImmersedInWater && !rootBlock.IsFireProof && !rootBlock.IsBurning)
                    flammableRoots.Add(rootBlock);
            }
            else if (Constants.PERISHABLE_BY_FIRE.Contains(interactObjects[key].EntityType))
            {
                Debug.Log($"Detected object perishable by fire: {interactObjects[key]}, {interactObjects[key].EntityType}");
                interactObjects[key].DestroyThis();
            }
        }

        if (flammableRoots.Count > 0)
        {
            Debug.Log("Non-zero number of flammable roots");
            FireSource fireSource = new FireSource();

            foreach (RootBlock rootBlock in flammableRoots)
            {
                rootBlock.SetOnFire();
                fireSource.AddRootBlock(rootBlock);    
            }

            GameController.Instance.AddFireSource(fireSource);
        }

        Debug.Log($"Contact after movement resolved: {this}");
    }
    public override void RunRootInteractionProcess(params RootBlock[] rootBlocks)
    {
        List<RootBlock> flammableRoots = new List<RootBlock>();

        foreach (RootBlock rootBlock in rootBlocks)
            if (!rootBlock.IsImmersedInWater && !rootBlock.IsFireProof && !rootBlock.IsBurning)
                flammableRoots.Add(rootBlock);

        if (flammableRoots.Count > 0)
        {
            Debug.Log("Non-zero number of flammable roots");
            FireSource fireSource = new FireSource();

            foreach (RootBlock rootBlock in flammableRoots)
            {
                rootBlock.SetOnFire();
                fireSource.AddRootBlock(rootBlock);
            }

            GameController.Instance.AddFireSource(fireSource);
        }

        Debug.Log($"Root interaction process ran: {this}");
    }
}
