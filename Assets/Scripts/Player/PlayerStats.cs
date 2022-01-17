using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerStats : EventListener
{
    public int collectedGems;
    public bool isPureGemCollected;


    private void Start()
    {
        targetEvent = OnCollected;
    }

    private void OnCollected(EventData obj)
    {
        if (obj.Code == EventCode._GEMCOLLECT_EVENTCODE)
        {
            object[] datas = (object[]) obj.CustomData;
            bool _isPureGemCollected = (bool) datas[1];

            // pure gem collected event
            if (_isPureGemCollected)
            {
                isPureGemCollected = true;
            }
            // other element gem collected event
            else if (!photonView.IsMine)
            {
                collectedGems++;
            }
        }
    }
}
