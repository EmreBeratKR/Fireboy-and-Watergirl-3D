using UnityEngine;
using NaughtyAttributes;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField, AnimatorParam("animator")] private string idle;
    [SerializeField, AnimatorParam("animator")] private string run;
    [SerializeField, AnimatorParam("animator")] private string push;
    [SerializeField, AnimatorParam("animator")] private string floating;


    private void Update()
    {
        Update_Animation();
    }

    private void Update_Animation()
    {
        switch (playerMovement.state)
        {
            case PlayerState.Idle:
                animator.SetBool(idle, true);
                animator.SetBool(run, false);
                animator.SetBool(push, false);
                animator.SetBool(floating, false);
                break;
            case PlayerState.Run:
                animator.SetBool(idle, false);
                animator.SetBool(run, true);
                animator.SetBool(push, false);
                animator.SetBool(floating, false);
                break;
            case PlayerState.Push:
                animator.SetBool(idle, false);
                animator.SetBool(run, false);
                animator.SetBool(push, true);
                animator.SetBool(floating, false);
                break;
            case PlayerState.Float:
                animator.SetBool(idle, false);
                animator.SetBool(run, false);
                animator.SetBool(push, false);
                animator.SetBool(floating, true);
                break;
        }
    }
}

public enum PlayerState {Idle, Run, Push, Float}
