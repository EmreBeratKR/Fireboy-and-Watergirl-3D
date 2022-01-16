using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSensor : MonoBehaviour
{
    [SerializeField] private Collider box;
    [SerializeField] private List<string> targets;
    private RaycastHit hit;
    private const float maxDistance = 10f;
    public Transform elevator;
    public Vector3 limit;


    private void FixedUpdate()
    {
        if (Physics.BoxCast(transform.position, box.bounds.size * 0.5f, Vector3.down, out hit, Quaternion.identity, maxDistance))
        {
            if (targets.Contains(hit.collider.gameObject.tag))
            {
                limit = transform.position + Vector3.down * hit.distance + Vector3.up * (elevator.lossyScale.y - box.bounds.size.y) * 0.5f;
                return;
            }
        }
        limit = transform.position + Vector3.down * maxDistance;
    }
}