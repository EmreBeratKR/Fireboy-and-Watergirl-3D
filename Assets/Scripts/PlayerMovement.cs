using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private KeyBinding keyBinding;
    public const float maxSpeed = 35f;


    private void Update()
    {
        if (photonView.IsMine)
        {
            // move right
            if (Input.GetKey(keyBinding.right[0]) || Input.GetKey(keyBinding.right[1]))
            {
                if (!(Mathf.Abs(rb.velocity.x) > maxSpeed))
                {
                    rb.AddForce(Vector3.right * 50f, ForceMode.Acceleration);
                }
            }
            else if ((Input.GetKeyUp(keyBinding.right[0]) || Input.GetKeyUp(keyBinding.right[1])) && rb.velocity.x > 0)
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
            // move left
            if (Input.GetKey(keyBinding.left[0]) || Input.GetKey(keyBinding.left[1]))
            {
                if (!(Mathf.Abs(rb.velocity.x) > maxSpeed))
                {
                    rb.AddForce(Vector3.left * 50f, ForceMode.Acceleration);
                }
            }
            else if ((Input.GetKeyUp(keyBinding.left[0]) || Input.GetKeyUp(keyBinding.left[1])) && rb.velocity.x < 0)
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
            // jump
            if (Input.GetKeyDown(keyBinding.jump[0]) || Input.GetKeyDown(keyBinding.jump[1]))
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
        if (velX > 0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else if (velX < -0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
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
