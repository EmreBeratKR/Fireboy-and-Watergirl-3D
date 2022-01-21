using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    private const float maxDistance = 2f;
    private RaycastHit hit;


    public bool isGrounded()
    {
        return Physics.BoxCast(transform.position, transform.lossyScale * 0.5f, Vector3.down, out hit, transform.rotation, maxDistance, targetLayer);
    }

    private void OnDrawGizmos()
    {
        if (isGrounded())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, transform.localScale);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * maxDistance);
            Gizmos.DrawWireCube(transform.position + Vector3.down * maxDistance, transform.localScale);
        }
    }
}
