using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbine : MonoBehaviour
{
    [SerializeField] private Transform blades;
    [SerializeField] private float windForce;


    private void Start()
    {
        StartCoroutine(RotateCo());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Box")
        {
            other.attachedRigidbody.AddForce(transform.up * windForce, ForceMode.Impulse);
        }
    }


    private IEnumerator RotateCo()
    {
        float duration = 0.5f;
        WaitForSeconds stepDuration =  new WaitForSeconds(duration * 0.25f);

        while (true)
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

    }
}
