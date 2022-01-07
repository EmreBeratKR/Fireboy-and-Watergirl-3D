using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Switch
{
    [SerializeField] private Transform lever;
    private bool isMoving = false;
    private const float duration = 1f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            if (isOn)
            {
                StartCoroutine(Close());
                return;
            }
            StartCoroutine(Open());
        }
    }


    public IEnumerator Open()
    {
        isMoving = true;
        lever.LeanRotateZ(60f, duration).setEaseOutSine();
        yield return new WaitForSeconds(duration);
        isOn = true;
        isMoving = false;
    }

    public IEnumerator Close()
    {
        isMoving = true;
        lever.LeanRotateZ(0f, duration).setEaseOutSine();
        yield return new WaitForSeconds(duration);
        isOn = false;
        isMoving = false;
    }
}
