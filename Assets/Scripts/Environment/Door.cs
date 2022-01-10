using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Door : EventListener
{
    [SerializeField] private Transform door;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    public Element element;
    public bool isOpen;
    private float progress = 0f;
    private bool inRange = false;


    private void Start()
    {
        targetEvent = OnDoorEvent;
    }

    private void FixedUpdate()
    {
        isOpen = false;
        progress += (inRange ? 1 : -1) * Time.fixedDeltaTime * speed;
        if (progress > 1)
        {
            progress = 1;
            isOpen = true;
        }
        else if (progress < 0)
        {
            progress = 0;
        }
        door.localPosition = Vector3.Lerp(Vector3.zero, target.localPosition, progress);
    }

    private void OnDoorEvent(EventData obj)
    {
        if (obj.Code == EventCode._DOOR_EVENTCODE)
        {
            object[] datas = (object[]) obj.CustomData;
            Element _element = (Element) datas[0];
            bool _inRange = (bool) datas[1];

            if (_element == element)
            {
                inRange = _inRange;
            }
        }
    }

    public void Raise_DoorEvent(bool state)
    {
        object[] datas = new object[] {element, state};
        PhotonNetwork.RaiseEvent(EventCode._DOOR_EVENTCODE, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        SetRange(state);
    }

    private void SetRange(bool state)
    {
        inRange = state;
    }
}
