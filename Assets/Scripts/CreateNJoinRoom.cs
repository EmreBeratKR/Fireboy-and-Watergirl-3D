using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class CreateNJoinRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInput;
    public TMP_InputField nameInput;
    [SerializeField] Text warningText;
    [SerializeField, Range(10, 20)] private int maxLength;
    [SerializeField] private string[] defaultNicknames;


    private void Start()
    {
        setInputField();
    }

    private void setInputField()
    {
        roomInput.characterLimit = maxLength;
        roomInput.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;

        nameInput.characterLimit = maxLength;
        nameInput.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
    }


    public void CreateRoom()
    {
        string c = roomInput.text;
        string n = nameInput.text;
        if (n == "")
        {
            warningText.text = "Please enter your Nickname!";
        }
        else if (c == "")
        {
            warningText.text = "Please enter a Room Name to Host!";
        }
        else
        {
            PhotonNetwork.CreateRoom(c, Room_Option(2));
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (returnCode == ErrorCode.GameIdAlreadyExists)
        {
            warningText.text = "Failed to create room, That room has already existed!";
            return;
        }
    }

    public void JoinRoom()
    {
        if (nameInput.text == "")
        {
            warningText.text = "Please enter your Nickname!";
        }
        else if (roomInput.text == "")
        {
            warningText.text = "Please enter a Room Name to Join!";
        }
        else
        {
            PhotonNetwork.JoinRoom(roomInput.text);
        }
    }

    public override void OnJoinedRoom()
    {
        SetNickname();
        PhotonNetwork.LoadLevel(2); // Room Scene
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        warningText.text = $"Failed to Join ({roomInput.text}), {message}!";
    }

    private void SetNickname()
    {
        string n = nameInput.text;
        if (n.Length == 0)
        {
            n = defaultNicknames[Random.Range(0, defaultNicknames.Length)];
        }
        else if (n.Length > maxLength)
        {
            n = n.Substring(0, maxLength);
        }
        PhotonNetwork.NickName = n;
    }

    private RoomOptions Room_Option(byte max)
    {
        RoomOptions opt = new RoomOptions();
        opt.MaxPlayers = max;
        return opt;
    }
}
