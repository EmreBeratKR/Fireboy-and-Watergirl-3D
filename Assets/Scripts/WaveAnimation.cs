using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAnimation : MonoBehaviour
{
    [SerializeField] private Transform waves;
    [SerializeField] private float duration;
    private const float deltaX = 10.54f;


    private void Start()
    {
        StartCoroutine(AnimationCo());
    }

    private IEnumerator AnimationCo()
    {
        while (true)
        {
            waves.LeanMoveLocalX(deltaX, duration);
            yield return new WaitForSeconds(duration);
            waves.localPosition = Vector3.zero;
        }
    }
}
