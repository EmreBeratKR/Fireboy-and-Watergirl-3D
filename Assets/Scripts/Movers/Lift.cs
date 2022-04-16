using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Switch[] switches;
    [SerializeField] private SwitchMode mode;
    [SerializeField] private MotionMode motionMode;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    private float progress = 0;
    private bool isMakeNoise = false;
    private bool isMoving;
    public bool isLocked;
    public bool safeDirection;


    private void FixedUpdate()
    {
        if (!isLocked || (State() == safeDirection))
        {
            isMoving = true;

            progress += (State() ? 1 : -1) * Time.fixedDeltaTime * speed;
            if (progress > 1)
            {
                progress = 1;
                isMoving = false;
            }
            else if (progress < 0)
            {
                progress = 0;
                isMoving = false;
            }

            switch (motionMode)
            {
                case MotionMode.Move:
                    rb.MovePosition(Vector3.Lerp(transform.position, target.position, progress));
                    break;
                case MotionMode.Rotate:
                    rb.MoveRotation(Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, target.rotation.eulerAngles, progress)));
                    break;
            }
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            if (!isMakeNoise)
            {
                AudioManager.TryPlayLiftMove();
                isMakeNoise = true;
            }
        }
        else
        {
            if (isMakeNoise)
            {
                AudioManager.TryStopLiftMove();
                isMakeNoise = false;
            }
        }
    }

    private bool State()
    {
        if (mode == SwitchMode.One)
        {
            for (int i = 0; i < switches.Length; i++)
            {
                if (switches[i].isOn) return true;
            }
            return false;
        }
        else if (mode == SwitchMode.All)
        {
            for (int i = 0; i < switches.Length; i++)
            {
                if (!switches[i].isOn) return false;
            }
            return true;
        }
        return false;
    }
}

public enum SwitchMode {One, All, Always}
public enum MotionMode {Move, Rotate}
