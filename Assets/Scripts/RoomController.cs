using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private GameObject startButton;
    [SerializeField] Text playerCounter;
    [SerializeField] Text roomName;
    [SerializeField] GameObject playerDisplay;
    [SerializeField] RectTransform playerSlots;


    private void Start()
    {
        OnClientEnteredRoom();
    }

    private void OnClientEnteredRoom()
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);

        PhotonNetwork.EnableCloseConnection = true;
        PhotonNetwork.AutomaticallySyncScene = true;
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        StartCoroutine(UpdateTable(PhotonNetwork.LocalPlayer));
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LocalPlayer.NickName = "";
        sceneController.LoadLobby();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(UpdateTable(newPlayer));
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
        UpdatePlayerCounter();
    }

    private IEnumerator UpdateTable(Player newPlayer)
    {
        while (true)
        {
            if (newPlayer.NickName != "")
            {
                break;
            }
            yield return 0;
        }
        UpdatePlayerList();
        UpdatePlayerCounter();
    }


    private void UpdatePlayerList()
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < playerSlots.childCount; i++)
        {
            if (playerSlots.GetChild(i).childCount != 0)
            {
                Destroy(playerSlots.GetChild(i).GetChild(0).gameObject);
                if (i < players.Length)
                {
                    GameObject newDisplay = Instantiate(playerDisplay, Vector3.zero, Quaternion.identity, playerSlots.GetChild(i));
                    newDisplay.transform.localPosition = Vector3.zero;
                    newDisplay.transform.GetChild(0).GetComponent<Text>().text = players[i].NickName;

                    newDisplay.GetComponent<ClientController>().owner = players[i];
                    if (PhotonNetwork.IsMasterClient && !players[i].IsLocal)
                    {
                        newDisplay.transform.Find("Kick Button").gameObject.SetActive(true);
                    }
                    else
                    {
                        newDisplay.transform.Find("Kick Button").gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (i < players.Length)
                {
                    GameObject newDisplay = Instantiate(playerDisplay, Vector3.zero, Quaternion.identity, playerSlots.GetChild(i));
                    newDisplay.transform.localPosition = Vector3.zero;
                    newDisplay.transform.GetChild(0).GetComponent<Text>().text = players[i].NickName;

                    newDisplay.GetComponent<ClientController>().owner = players[i];
                    if (PhotonNetwork.IsMasterClient && !players[i].IsLocal)
                    {
                        newDisplay.transform.Find("Kick Button").gameObject.SetActive(true);
                    }
                    else
                    {
                        newDisplay.transform.Find("Kick Button").gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void UpdatePlayerCounter()
    {
        playerCounter.text = $"Player {PhotonNetwork.PlayerList.Length.ToString()}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
    }
}
