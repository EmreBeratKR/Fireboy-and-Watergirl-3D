using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Switch[] switches;
    [SerializeField] private SwitchMode mode;
    [SerializeField] private Transform lift;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    private float progress = 0;


    private void FixedUpdate()
    {
        progress += (State() ? 1 : -1) * Time.fixedDeltaTime * speed;
        if (progress > 1)
        {
            progress = 1;
        }
        else if (progress < 0)
        {
            progress = 0;
        }
        rb.MovePosition(Vector3.Lerp(transform.position, target.position, progress));
        //lift.localPosition = Vector3.Lerp(transform.position, target.position, progress);
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

public enum SwitchMode {One, All}
