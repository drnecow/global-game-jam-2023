using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Project.Constants;

public class FireTrigger : MapObject
{
    [SerializeField] private List<Fire> _relatedFires;

    [SerializeField] private List<GameObject> _oldTrajectoriesObj;
    [SerializeField] private List<GameObject> _newTrajectoriesObj;

    private List<List<Animator>> _oldTrajectoryControllers;
    private List<List<Animator>> _newTrajectoryControllers;


    [SerializeField] private List<Coords> _trajectory1;
    [SerializeField] private List<Coords> _trajectory2;
    private List<List<Coords>> _newTrajectories;


    [SerializeField] Sprite _activeSprite;


    private void Awake()
    {
        _entityType = EntityType.FireTrigger;

        _oldTrajectoryControllers = new List<List<Animator>>() { new List<Animator>(), new List<Animator>() };
        _newTrajectoryControllers = new List<List<Animator>>() { new List<Animator>(), new List<Animator>() };

        _newTrajectories = new List<List<Coords>>
        {
            _trajectory1,
            _trajectory2
        };
    }
    private void Start()
    {
        for (int i = 0; i < _oldTrajectoriesObj.Count; i++)
        {
            GameObject trajectory = _oldTrajectoriesObj[i];

            foreach (Transform child in trajectory.transform)
            {
                Debug.Log(child.gameObject);
                Animator animator = child.gameObject.GetComponent<Animator>();
                _oldTrajectoryControllers[i].Add(animator);
                animator.SetBool("Active", true);
            }
        }

        for (int i = 0; i < _newTrajectoriesObj.Count; i++)
        {
            GameObject trajectory = _newTrajectoriesObj[i];

            foreach (Transform child in trajectory.transform)
            {
                Debug.Log(child.gameObject);
                Animator animator = child.gameObject.GetComponent<Animator>();
                _newTrajectoryControllers[i].Add(animator);
            }
        }
    }

    public override void RunRootInteractionProcess(params RootBlock[] rootBlocks)
    {
        GetComponent<SpriteRenderer>().sprite = _activeSprite;

        for (int i = 0; i < _relatedFires.Count; i++)
        {
            _relatedFires[i].SetNewTrajectory(_newTrajectories[i]);
            SetControllersInactive(_oldTrajectoryControllers[i]);
            SetControllersActive(_newTrajectoryControllers[i]);
        }
    }
    private void SetControllersActive(List<Animator> animators)
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("Active", true);
        }
    }
    private void SetControllersInactive(List<Animator> animators)
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("Active", false);
        }
    }
}
