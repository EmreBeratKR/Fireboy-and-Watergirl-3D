using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Rigidbody connectedRb;
    [SerializeField, Min(0f)] private float maxSpeed;
    private float speed;
    private float connectedSpeed;
    

    private void FixedUpdate()
    {
        speed = rb.velocity.magnitude;
        connectedSpeed = connectedRb.velocity.magnitude;

        if (speed > connectedSpeed)
        {
            rb.AddForce(-rb.velocity.normalized * (speed - connectedSpeed), ForceMode.VelocityChange);
            rb.velocity = rb.velocity.normalized * connectedSpeed;
        }
        if (speed > maxSpeed)
        {
            rb.AddForce(-rb.velocity.normalized * (speed - maxSpeed), ForceMode.VelocityChange);
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        Debug.Log(rb.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody otherBody))
        {
            otherBody.velocity = Vector3.zero;
        }
    }
}
