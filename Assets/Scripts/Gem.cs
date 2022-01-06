using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public GemType type;
    [SerializeField, Min(0f)] private float duration;
    [SerializeField, Min(0f)] private float deltaY;
    private Vector3 startPos;


    private void Start()
    {
        startPos = transform.localPosition;
        StartCoroutine(AnimationCo());
    }

    private IEnumerator AnimationCo()
    {
        WaitForSeconds wait = new WaitForSeconds(duration);

        while (true)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            
            transform.LeanRotateY(-90f, duration);
            transform.LeanMoveLocalY(startPos.y + deltaY, duration).setEaseOutSine();
            yield return wait;

            transform.LeanRotateY(-180f, duration);
            transform.LeanMoveLocalY(startPos.y, duration).setEaseInSine();
            yield return wait;

            transform.LeanRotateY(-270f, duration);
            transform.LeanMoveLocalY(startPos.y - deltaY, duration).setEaseOutSine();
            yield return wait;

            transform.LeanRotateY(0f, duration);
            transform.LeanMoveLocalY(startPos.y, duration).setEaseInSine();
            yield return wait;
        }
    }
}

public enum GemType {Fire, Water}
