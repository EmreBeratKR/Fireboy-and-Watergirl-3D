using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Box : MonoBehaviourPun
{

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<PhotonView>().Owner;
            if (photonView.Owner != player)
            {
                photonView.TransferOwnership(player);
            }
        }
    }
}
