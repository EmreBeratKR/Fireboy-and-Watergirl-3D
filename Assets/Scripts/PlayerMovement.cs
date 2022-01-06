using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;
    [SerializeField] private GroundChecker groundChecker;
    [SerializeField] private KeyBinding keyBinding;
    public const float maxSpeed = 35f;
    public const float moveTreshhold = 0.5f;


    private void Update()
    {
        if (photonView.IsMine)
        {
            rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y, 0f);
            UpdateFacing();

            // move right
            if (Input.GetKey(keyBinding.right[0]) || Input.GetKey(keyBinding.right[1]))
            {
                if (!(Mathf.Abs(rb.velocity.x) > maxSpeed))
                {
                    rb.AddForce(Vector3.right * 50f, ForceMode.Acceleration);
                    TryJump();
                    return;
                }
            }
            
            // move left
            if (Input.GetKey(keyBinding.left[0]) || Input.GetKey(keyBinding.left[1]))
            {
                if (!(Mathf.Abs(rb.velocity.x) > maxSpeed))
                {
                    rb.AddForce(Vector3.left * 50f, ForceMode.Acceleration);
                    TryJump();
                    return;
                }
            }

            TryJump();
            
            if (Mathf.Abs(rb.velocity.y) <= moveTreshhold || !groundChecker.isGrounded())
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
        }
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(keyBinding.jump[0]) || Input.GetKeyDown(keyBinding.jump[1]))
        {
            if (groundChecker.isGrounded())
            {
                rb.velocity = new Vector3(rb.velocity.x, 50f, 0f);
            }
        }
    }

    private void UpdateFacing()
    {
        float velX = rb.velocity.x;
        if (velX > moveTreshhold)
        {
            model.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else if (velX < -moveTreshhold)
        {
            model.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else
        {
            model.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}

[System.Serializable]
public struct KeyBinding
{
    public KeyCode[] right;
    public KeyCode[] left;
    public KeyCode[] jump;
}
