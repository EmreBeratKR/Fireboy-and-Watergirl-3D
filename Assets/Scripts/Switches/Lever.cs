using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Lever : Switch
{
    [SerializeField] private Transform lever;
    private bool isMoving = false;
    private const float duration = 1f;


    private void Start()
    {
        targetEvent = OnLeverEventReceived;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving && inRange)
        {
            Raise_LeverEvent();
        }
    }

    private void OnLeverEventReceived(EventData obj)
    {
        if (obj.Code == EventCode._LEVER_EVENTCODE)
        {
            object[] datas = (object[]) obj.CustomData;
            int viewID = (int) datas[0];
            bool state = (bool) datas[1];

            if (photonView.ViewID == viewID)
            {
                ToggleLever(state);
            }
        }
    }

    private void Raise_LeverEvent()
    {
        object[] datas = new object[] {photonView.ViewID, isOn};
        PhotonNetwork.RaiseEvent(EventCode._LEVER_EVENTCODE, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        ToggleLever(isOn);
    }

    private void ToggleLever(bool currentState)
    {
        if (currentState)
        {
            StartCoroutine(Close());
        }
        else
        {
            StartCoroutine(Open());
        }

        AudioManager.PlayLeverPull();
    }

    private IEnumerator Open()
    {
        isMoving = true;
        lever.LeanRotateZ(60f, duration).setEaseOutSine();
        yield return new WaitForSeconds(duration);
        isOn = true;
        isMoving = false;
    }

    private IEnumerator Close()
    {
        isMoving = true;
        lever.LeanRotateZ(0f, duration).setEaseOutSine();
        yield return new WaitForSeconds(duration);
        isOn = false;
        isMoving = false;
    }
}
