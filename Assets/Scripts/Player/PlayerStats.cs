using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerStats : EventListener
{
    public int collectedGems;
    public bool isBigGemCollected;


    private void Start()
    {
        targetEvent = OnCollected;
    }

    private void OnCollected(EventData obj)
    {
        if (obj.Code == EventCode._GEMCOLLECT_EVENTCODE)
        {
            if (!photonView.IsMine)
            {
                collectedGems++;
            }
        }
    }
}
