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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PhotonView view))
        {
            if (view.IsMine)
            {
                Raise_PushButtonEvent(true, true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PhotonView view))
        {
            if (view.IsMine)
            {
                Raise_PushButtonEvent(true, false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PhotonView view))
        {
            if (view.IsMine)
            {
                Raise_PushButtonEvent(false, true);
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
            bool playAudio = (bool) datas[2];

            if (photonView.ViewID == viewID)
            {
                SetPushButtonState(newState, playAudio);
            }
        }
    }

    private void Raise_PushButtonEvent(bool newState, bool playAudio)
    {
        object[] datas = new object[] {photonView.ViewID, newState, playAudio};
        PhotonNetwork.RaiseEvent(EventCode._PUSHBUTTON_EVENTCODE, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        SetPushButtonState(newState, playAudio);
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

    private void SetPushButtonState(bool newState, bool playAudio)
    {
        if (newState)
        {
            Open();
        }
        else
        {
            Close();
        }

        if (playAudio)
        {
            AudioManager.PlayButtonToggle();
        }
    }
}
