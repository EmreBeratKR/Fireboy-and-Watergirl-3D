using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ChatBox : EventListener
{
    [SerializeField] private PhotonView view;
    [SerializeField] private InputField chatbox;
    private bool isWriting = false;
    private float lastMsg_Time = 0f;
    private const float msgLifetime = 3f;
    

    private void Start()
    {
        targetEvent = OnChatEvent;
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isWriting = !isWriting;
                chatbox.gameObject.SetActive(isWriting);
                view.GetComponent<PlayerMovement>().isChatting = isWriting;
                if (isWriting)
                {
                    chatbox.ActivateInputField();
                    chatbox.text = "";
                }
                else
                {
                    if (chatbox.text != "")
                    {
                        Raise_ChatEvent(chatbox.text);
                    }
                }
            }
        }

        if (Time.time - lastMsg_Time > msgLifetime && !isWriting)
        {
            chatbox.gameObject.SetActive(false);
        }
    }

        private void OnChatEvent(EventData obj)
    {
        if (obj.Code == EventCode._CHATMSG_EVENTCODE)
        {
            object[] datas = (object[]) obj.CustomData;
            int sender_viewID = (int) datas[0];
            string _msg = (string) datas[1];

            if (view.ViewID == sender_viewID)
            {
                chatbox.text = _msg;
                chatbox.gameObject.SetActive(true);
                lastMsg_Time = Time.time;
            }
        }
    }

    public void Raise_ChatEvent(string msg)
    {
        object[] datas = new object[] {view.ViewID, msg};
        PhotonNetwork.RaiseEvent(EventCode._CHATMSG_EVENTCODE, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        chatbox.gameObject.SetActive(true);
        lastMsg_Time = Time.time;
    }
}
