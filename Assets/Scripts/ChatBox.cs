using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ChatBox : EventListener
{
    private const string invalidChars = "\n";
    private const float msgLifetime = 3f;
    private const float extraLineHeight = 15f;

    [SerializeField] private PhotonView view;
    [SerializeField] private TMP_InputField chatbox;
    private RectTransform rectTransform;
    private bool isWriting = false;
    private float lastMsgTime = 0f;
    private float singleLineHeight;

    private float height { get => rectTransform.sizeDelta.y; set => rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, value); }
    private int lineCount => chatbox.textComponent.GetTextInfo(chatbox.text).lineCount;
    

    private void Start()
    {
        targetEvent = OnChatEvent;
        rectTransform = chatbox.GetComponent<RectTransform>();
        singleLineHeight = height;

        if (!view.IsMine)
        {
            chatbox.readOnly = true;
            chatbox.interactable = false;
        }
    }

    private void OnEnable()
    {
        MobileOnlyEventSystem.OnChatButtonPressed += OnChatButtonPressed;
        MobileOnlyEventSystem.OnEnterTouchKeyPressed += OnEnterTouchKeyPressed;
    }

    private void OnDisable()
    {
        MobileOnlyEventSystem.OnChatButtonPressed -= OnChatButtonPressed;
        MobileOnlyEventSystem.OnEnterTouchKeyPressed -= OnEnterTouchKeyPressed;
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isWriting = !isWriting;
                view.GetComponent<PlayerMovement>().isChatting = isWriting;
                if (isWriting)
                {
                    ShowChatBox();
                    chatbox.ActivateInputField();
                    chatbox.text = "";
                }
                else
                {
                    if (chatbox.text != "")
                    {
                        Raise_ChatEvent(chatbox.text);
                    }
                    
                    chatbox.readOnly = true;
                    chatbox.interactable = false;
                }
            }

            if (isWriting)
            {
                UpdateHeight();
            }
        }

        TrySelfHide();
    }

    private void OnChatButtonPressed()
    {
        isWriting = !isWriting;
        view.GetComponent<PlayerMovement>().isChatting = isWriting;

        if (isWriting)
        {
            StartWriting();
            return;
        }

        CancelWriting();
    }

    private void OnEnterTouchKeyPressed()
    {
        SubmitWriting();
    }

    private void StartWriting()
    {
        ShowChatBox();
        chatbox.ActivateInputField();
        chatbox.text = "";

        TouchKeyboard.Show();
    }

    private void SubmitWriting()
    {
        isWriting = false;
        view.GetComponent<PlayerMovement>().isChatting = false;
        
        TouchKeyboard.Hide();

        if (chatbox.text != "")
        {
            Raise_ChatEvent(chatbox.text);
            return;
        }

        CancelWriting();   
    }

    private void CancelWriting()
    {
        HideChatBox();
        chatbox.readOnly = true;
        chatbox.interactable = false;

        TouchKeyboard.Hide();
    }

    private void ShowChatBox()
    {
        chatbox.readOnly = false;
        chatbox.interactable = true;
        chatbox.gameObject.SetActive(true);
    }

    private void HideChatBox()
    {
        chatbox.gameObject.SetActive(false);
    }

    private string IgnoreInvalidChars(string input)
    {
        var filtered = "";

        foreach (char ch in input)
        {
            if (invalidChars.Contains(ch)) continue;

            filtered += ch;
        }

        return filtered;
    }

    private bool TrySelfHide()
    {
        if ((Time.time - lastMsgTime) < msgLifetime) return false;

        if (isWriting) return false;

        HideChatBox();
        return true;
    }

    private void UpdateHeight()
    {
        this.height = singleLineHeight + extraLineHeight * (lineCount-1);
    }

    public void OnValueChanged()
    {
        chatbox.text = IgnoreInvalidChars(chatbox.text);
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
                UpdateHeight();
                ShowChatBox();
                lastMsgTime = Time.time;
            }
        }
    }

    public void Raise_ChatEvent(string msg)
    {
        object[] datas = new object[] {view.ViewID, msg};
        PhotonNetwork.RaiseEvent(EventCode._CHATMSG_EVENTCODE, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        lastMsgTime = Time.time;
    }
}
