using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject warningText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameoverScreen;
    private const int levelDelta = 3;
    private const float gameoverWait = 1.5f;


    public void LoadLobby()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevelSelection()
    {
        if (PhotonNetwork.PlayerList.Length == 2 || true)
        {
            PhotonNetwork.LoadLevel(3); // Level Selection Scene
        }
        else
        {
            warningText.SetActive(true);
        }
    }

    public void Load_Level(int levelNumber)
    {
        PhotonNetwork.LoadLevel(levelNumber+levelDelta);
    }

    public void Restart_Level()
    {
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void Toggle_PauseMenu(bool isOpen)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pauseMenu.SetActive(isOpen);
        }
    }

    public void Open_GameoverScreen()
    {
        StartCoroutine(Open_GameoverScreenCo());
    }

    private IEnumerator Open_GameoverScreenCo()
    {
        yield return new WaitForSeconds(gameoverWait);
        gameoverScreen.SetActive(true);
    }
}
