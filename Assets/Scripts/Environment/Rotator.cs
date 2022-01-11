using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    
    private void FixedUpdate()
    {
        Debug.Log(rb.rotation.eulerAngles + Vector3.forward * speed * Time.fixedDeltaTime);
        rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + Vector3.forward * speed * Time.fixedDeltaTime));
    }
}
