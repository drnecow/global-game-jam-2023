using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineAnimation : MonoBehaviour
{
    public static CoroutineAnimation Instance { get; private set; }

    [SerializeField] private float _animationDuration = 0.3f;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("CoroutineAnimation instance already exists, destroying this");
            Destroy(this);
        }
        else
            Instance = this;
    }

    public void MoveTo(GameObject obj, Vector3 destinationPos)
    {
        StartCoroutine(AnimateMovement(obj, destinationPos));
    }

    public IEnumerator AnimateMovement(GameObject obj, Vector3 destinationPos)
    {
        Vector3 startPos = obj.transform.position;
        float progress = 0;

        while (progress <= 1)
        {
            obj.transform.position = Vector3.Lerp(startPos, destinationPos, progress);
            progress += Time.deltaTime / _animationDuration;
            yield return null;
        }

        obj.transform.position = destinationPos;
    }
}
