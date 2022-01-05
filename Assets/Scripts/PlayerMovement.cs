using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] private Rigidbody rb;
    public const float maxSpeed = 30f;


    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.D))
            {
                if (!(Mathf.Abs(rb.velocity.x) > maxSpeed))
                {
                    rb.AddForce(Vector3.right * 50f, ForceMode.Acceleration);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (!(Mathf.Abs(rb.velocity.x) > maxSpeed))
                {
                    rb.AddForce(Vector3.left * 50f, ForceMode.Acceleration);
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                rb.velocity = new Vector3(rb.velocity.x, 50f, 0f);
            }
            rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y, 0f);
            UpdateFacing();
        }
    }

    private void UpdateFacing()
    {
        float velX = rb.velocity.x;
        if (velX > 0.5f)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else if (velX < -0.5f)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
