using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CollisionController : MonoBehaviourPun
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Element element;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gem")
        {
            if (photonView.IsMine)
            {
                Gem gem = other.GetComponent<Gem>();
                if (gem.element == element)
                {
                    gem.RaiseGemEvent();
                }
            }
        }
        else if (other.tag == "Switch")
        {
            if (photonView.IsMine)
            {
                other.GetComponent<Switch>().inRange = true;
            }
        }
        else if (other.tag == "Door")
        {
            Door door = other.GetComponent<Door>();
            if (element == door.element)
            {
                door.Raise_DoorEvent(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Switch")
        {
            if (photonView.IsMine)
            {
                other.GetComponent<Switch>().inRange = false;
            }
        }
        else if (other.tag == "Door")
        {
            Door door = other.GetComponent<Door>();
            if (element == door.element)
            {
                door.Raise_DoorEvent(false);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Lift")
        {
            playerMovement.isLifted = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Lift")
        {
            playerMovement.isLifted = false;
        }
    }
}
