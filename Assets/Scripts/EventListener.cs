using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class EventListener : MonoBehaviourPun
{
    public delegate void EventDel(EventData obj);
    public EventDel targetEvent;
    
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }

    private void OnEventReceived(EventData obj)
    {
        if (targetEvent == null) return;
        targetEvent(obj);
    }
}
