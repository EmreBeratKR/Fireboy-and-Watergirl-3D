using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PushButton : Switch
{
    [SerializeField] private Transform button;


    private void Start()
    {
        targetEvent = OnPushButtonEventReceived;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PhotonView view))
        {
            if (view.IsMine)
            {
                Raise_PushButtonEvent(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PhotonView view))
        {
            if (view.IsMine)
            {
                Raise_PushButtonEvent(false);
            }
        }
    }

    private void OnPushButtonEventReceived(EventData obj)
    {
        if (obj.Code == EventCode._PUSHBUTTON_EVENTCODE)
        {
            object[] datas = (object[]) obj.CustomData;
            int viewID = (int) datas[0];
            bool newState = (bool) datas[1];

            if (photonView.ViewID == viewID)
            {
                SetPushButtonState(newState);
            }
        }
    }

    private void Raise_PushButtonEvent(bool newState)
    {
        object[] datas = new object[] {photonView.ViewID, newState};
        PhotonNetwork.RaiseEvent(EventCode._PUSHBUTTON_EVENTCODE, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        SetPushButtonState(newState);
    }

    private void Open()
    {
        isOn = true;
        button.transform.localPosition = Vector3.up * 0.021f;
    }

    private void Close()
    {
        isOn = false;
        button.transform.localPosition = Vector3.up * 0.028f;
    }

    private void SetPushButtonState(bool newState)
    {
        if (newState)
        {
            Open();
        }
        else
        {
            Close();
        }
    }
}
