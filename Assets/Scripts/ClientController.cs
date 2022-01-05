using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ClientController : MonoBehaviour
{
    [SerializeField] private string ownerNickname; // just for easy debugging
    public Player owner;


    private void Start()
    {
        ownerNickname = owner.NickName;
    }

    public void KickPlayer()
    {
        PhotonNetwork.CloseConnection(owner);
    }
}
