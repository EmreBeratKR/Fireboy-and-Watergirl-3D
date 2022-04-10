using UnityEngine;

public class SeeSaw : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField, Range(0, 90)] private float maxAngle;

    private float angle => body.rotation.eulerAngles.z;
    
    private void FixedUpdate()
    {
        body.isKinematic = false;

        if (angle <= 180)
        {
            if (angle > maxAngle)
            {
                body.isKinematic = true;
                body.MoveRotation(Quaternion.Euler(Vector3.forward * maxAngle));
            }
        }
        else
        {
            if ((360 - maxAngle) > angle)
            {
                body.isKinematic = true;
                body.MoveRotation(Quaternion.Euler(Vector3.forward * (360 - maxAngle)));
            }
        }
    }
}
