using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject warningText;


    public void LoadLobby()
    {
        SceneManager.LoadScene(1);
    }

    public void EnterGame()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            PhotonNetwork.LoadLevel(3); // Game Scene
        }
        else
        {
            warningText.SetActive(true);
        }
    }
}
