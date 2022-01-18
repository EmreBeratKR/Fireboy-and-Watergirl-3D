using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbine : MonoBehaviour
{
    [SerializeField] private Transform blades;
    [SerializeField] private Switch[] switches;
    [SerializeField] private SwitchMode mode;
    [SerializeField] private float windForce;
    [SerializeField] private bool isActive;
    [SerializeField] private RotationAxis rotationAxis;


    private void Start()
    {
        if (isActive)
        {
            StartCoroutine(RotateCo());
        }
    }

    private void FixedUpdate()
    {
        if (State())
        {
            if (!isActive)
            {
                isActive = true;
                StartCoroutine(RotateCo());
            }
        }
        else
        {
            if (isActive)
            {
                isActive = false;
                StopAllCoroutines();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActive)
        {
            if (other.tag == "Player" || other.tag == "Box")
            {
                other.attachedRigidbody.AddForce(transform.up * windForce, ForceMode.Impulse);
            }
        }
    }

    private bool State()
    {
        if (mode == SwitchMode.One)
        {
            for (int i = 0; i < switches.Length; i++)
            {
                if (switches[i].isOn) return true;
            }
            return false;
        }
        else if (mode == SwitchMode.All)
        {
            for (int i = 0; i < switches.Length; i++)
            {
                if (!switches[i].isOn) return false;
            }
            return true;
        }
        return false;
    }

    private IEnumerator RotateCo()
    {
        float duration = 0.5f;
        WaitForSeconds stepDuration =  new WaitForSeconds(duration * 0.25f);

        while (true)
        {     
            if (rotationAxis == RotationAxis.Y)
            {
                blades.LeanRotateY(90f, duration * 0.25f);
                yield return stepDuration;
                blades.LeanRotateY(180f, duration * 0.25f);
                yield return stepDuration;
                blades.LeanRotateY(270f, duration * 0.25f);
                yield return stepDuration;
                blades.LeanRotateY(0f, duration * 0.25f);
                yield return stepDuration;
            }
            if (rotationAxis == RotationAxis.X)
            {
                blades.LeanRotateX(90f, duration * 0.25f);
                yield return stepDuration;
                blades.LeanRotateX(180f, duration * 0.25f);
                yield return stepDuration;
                blades.LeanRotateX(270f, duration * 0.25f);
                yield return stepDuration;
                blades.LeanRotateX(0f, duration * 0.25f);
                yield return stepDuration;
            }
        }
    }
}

public enum RotationAxis {X, Y}