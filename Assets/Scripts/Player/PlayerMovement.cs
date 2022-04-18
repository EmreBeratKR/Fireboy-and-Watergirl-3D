using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerMovement : EventListener
{
    [field:SerializeField] public Element element;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;
    [SerializeField] private GroundChecker groundChecker;
    [SerializeField] private CollisionController collisionController;
    [SerializeField] private KeyBinding keyBinding;
    [SerializeField, Range(10f, 50f)] private float rotationSpeed;
    private bool isRight = false;
    private bool isLeft = false;
    private bool isJump = false;
    private const float maxSpeed = 35f;
    private const float jumpSpeed = 75f;
    private const float slidingTreshhold = 1.5f;
    private const float teleportTreshhold_X = 3f;
    private const float teleportTreshhold_Y = 35f;
    public PlayerState state {get {return Get_State();}}
    public bool isLifted;
    public bool isLocked;
    public bool isChatting;
    public bool debugMode;


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

        UpdateFacing();

        Move();
    }

    private void GetInput()
    {
        isRight = (Input.GetKey(keyBinding.right[0]) || Input.GetKey(keyBinding.right[1]) || TouchInput.IsRight) && !isLocked && !isChatting;
        isLeft = (Input.GetKey(keyBinding.left[0]) || Input.GetKey(keyBinding.left[1]) || TouchInput.IsLeft) && !isLocked && !isChatting;
        isJump = (Input.GetKey(keyBinding.jump[0]) || Input.GetKey(keyBinding.jump[1]) || TouchInput.IsJump) && !isLocked && !isChatting;
    }

    private void TryJump()
    {
        if (isJump)
        {
            if (groundChecker.isGrounded() || debugMode)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, 0f);
                
                AudioManager.PlayJump(this.element);
            }
        }
    }

    private void Move()
    {
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
        float desiredAngle = 0;

        if (isRight)
        {
            desiredAngle = -90;
        }
        else if (isLeft)
        {
            desiredAngle = 90;
        }

        float lerpedAngle = Mathf.LerpAngle(model.eulerAngles.y, desiredAngle, rotationSpeed * Time.deltaTime);

        model.eulerAngles = Vector3.up * lerpedAngle;
    }

    private PlayerState Get_State()
    {
        if (!groundChecker.isGrounded())
        {
            return PlayerState.Float;
        }

        if (isLeft || isRight)
        {
            if (collisionController.isPushing)
            {
                return PlayerState.Push;
            }
            return PlayerState.Run;
        }
        return PlayerState.Idle;
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

                if ((Mathf.Abs(pos.x - transform.position.x) > teleportTreshhold_X) || (Mathf.Abs(pos.y - transform.position.y) > teleportTreshhold_Y) ||
                    (!_isRight && !_isLeft && !_isJump && groundChecker.isGrounded() && !isLifted))
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

[System.Serializable]
public struct KeyBinding
{
    public KeyCode[] right;
    public KeyCode[] left;
    public KeyCode[] jump;
}