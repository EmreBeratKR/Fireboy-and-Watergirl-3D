using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftSensor : MonoBehaviour
{
    [SerializeField] private Lift lift;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Box")
        {
            lift.isLocked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Box")
        {
            lift.isLocked = false;
        }
    }
}
