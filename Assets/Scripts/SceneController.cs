using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class SceneController : MonoBehaviour
{
    public void LoadLobby()
    {
        SceneManager.LoadScene(1);
    }

    public void EnterGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(3); // Game Scene
        }
    }
}
