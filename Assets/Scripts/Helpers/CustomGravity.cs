using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    public float gravityScale = 1.0f;
    private float baseGravity = 9.81f;
    private Rigidbody targetRigidbody;


    private void OnEnable()
    {
        targetRigidbody = GetComponent<Rigidbody>();
        // ignores the default rigidbody gravity
        targetRigidbody.useGravity = false;
    }
     
    private void FixedUpdate()
    {
        Vector3 gravity = baseGravity * gravityScale * Vector3.down;
        targetRigidbody.AddForce(gravity, ForceMode.Acceleration);
    }


}
