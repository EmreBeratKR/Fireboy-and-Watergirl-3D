using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CollisionController : MonoBehaviourPun
{
    [SerializeField] private GemType targetGem;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gem")
        {
            if (photonView.IsMine)
            {
                Gem gem = other.GetComponent<Gem>();
                if (gem.type == targetGem)
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
    }
}
