using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerMovement : EventListener
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;
    [SerializeField] private GroundChecker groundChecker;
    [SerializeField] private KeyBinding keyBinding;
    private Facing facing = Facing.Forward;
    private bool isRight = false;
    private bool isLeft = false;
    private bool isJump = false;
    private const float maxSpeed = 35f;
    private const float jumpSpeed = 75f;
    private const float moveTreshhold = 1f;
    private const float slidingTreshhold = 1.5f;
    private const float teleportTreshhold = 3f;
    private const float facingDuration = 0.5f;
    public bool isLifted;
    public bool debugMode; // for debugging


    private void Start()
    {
        targetEvent = OnMoveInputReceived;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            GetInput();
            Raise_MoveInputEvent();
        }

        Move();
    }

    private void GetInput()
    {
        isRight = Input.GetKey(keyBinding.right[0]) || Input.GetKey(keyBinding.right[1]);
        isLeft = Input.GetKey(keyBinding.left[0]) || Input.GetKey(keyBinding.left[1]);
        isJump = Input.GetKey(keyBinding.jump[0]) || Input.GetKeyDown(keyBinding.jump[1]);
    }

    private void TryJump()
    {
        if (isJump)
        {
            if (groundChecker.isGrounded() || debugMode)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, 0f);
            }
        }
    }

    private void Move()
    {
        UpdateFacing();
        if (isRight)
        {
            rb.velocity = new Vector3(maxSpeed, rb.velocity.y, 0f);
        }
        else if (isLeft)
        {
            rb.velocity = new Vector3(-maxSpeed, rb.velocity.y, 0f);
        }
        TryJump();
        if (!isRight && !isLeft)
        {
            if (Mathf.Abs(rb.velocity.y) <= slidingTreshhold || !groundChecker.isGrounded() || isLifted)
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
        }
    }
    private void UpdateFacing()
    {
        if (isRight)
        {
            facing = Facing.Right;
            model.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else if (isLeft)
        {
            facing = Facing.Left;
            model.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (!isRight && !isLeft)
        {
            facing = Facing.Left;
            model.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnMoveInputReceived(EventData obj)
    {
        if (obj.Code == EventCode._MOVEINPUT_EVENTCODE)
        {
            if (!photonView.IsMine)
            {
                object[] datas = (object[]) obj.CustomData;
                bool _isRight = (bool) datas[0];
                bool _isLeft = (bool) datas[1];
                bool _isJump = (bool) datas[2];
                Vector2 pos = new Vector2((float) datas[3], (float) datas[4]);

                if (Mathf.Abs(pos.x - transform.position.x) > teleportTreshhold || (!_isRight && !_isLeft && !_isJump && groundChecker.isGrounded() && !isLifted))
                {
                    rb.MovePosition(pos);
                }

                isRight = _isRight;
                isLeft = _isLeft;
                isJump = _isJump;
            }
        }
    }

    private void Raise_MoveInputEvent()
    {
        object[] datas = new object[] {isRight, isLeft, isJump, transform.position.x, transform.position.y};
        PhotonNetwork.RaiseEvent(EventCode._MOVEINPUT_EVENTCODE, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
}

public enum Facing {Forward, Right, Left}

[System.Serializable]
public struct KeyBinding
{
    public KeyCode[] right;
    public KeyCode[] left;
    public KeyCode[] jump;
}
