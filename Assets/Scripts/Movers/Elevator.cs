using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform rope;
    public Vector3 velocity;


    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        rope.localScale = new Vector3(1f, rope.position.y - transform.position.y, 1f);
    }
}
