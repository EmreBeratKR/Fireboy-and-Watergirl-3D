using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;


    private void Update()
    {
        Update_Animation();
    }

    private void Update_Animation()
    {
        switch (playerMovement.state)
        {
            case PlayerState.Idle:
                animator.Play("Character_Idle");
                break;
            case PlayerState.Run:
                animator.Play("Character_Run");
                break;
            case PlayerState.Push:
                animator.Play("Character_Push");
                break;
            case PlayerState.Float:
                animator.Play("Character_Float");
                break;
        }
    }
}

public enum PlayerState {Idle, Run, Push, Float}
