using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class SceneController : EventListener
{
    [SerializeField] private GameObject warningText;
    [SerializeField] private GameObject[] masterOnlyButtons;
    [SerializeField] private GameObject[] waitForMasterTexts;
    [SerializeField] private Text timer;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject resultScreen;
    [SerializeField] private GameObject[] resultMembers;
    [SerializeField] private GameObject gameoverScreen;
    [SerializeField] private Expectation expectations;
    private Spawner spawner;
    private float startTime;
    public const int levelDelta = 4;
    private const float menuWait = 1.5f;


    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > levelDelta)
        {
            startTime = Time.time;
            spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
            SetInGameCanvas();
        }
    }

    private void SetInGameCanvas()
    {
        foreach (var button in masterOnlyButtons)
        {
            button.SetActive(PhotonNetwork.IsMasterClient);
        }
        foreach (var text in waitForMasterTexts)
        {
            text.SetActive(!PhotonNetwork.IsMasterClient);
        }
    }

    public void LoadLobby()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevelSelection()
    {
        if (PhotonNetwork.IsMasterClient)
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
    }

    public void Load_Level(int levelNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(levelNumber+levelDelta);
        }
    }

    public void Restart_Level()
    {
        PlayerPrefs.SetInt("Restarted Level", SceneManager.GetActiveScene().buildIndex);
        PhotonNetwork.LoadLevel(4);
    }

    public void Toggle_PauseMenu(bool isOpen)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            spawner.Get_Me().GetComponent<PlayerMovement>().isLocked = isOpen;
            pauseMenu.SetActive(isOpen);
        }
    }

    public void Open_ResultScreen()
    {
        Set_ResultScreen();
        spawner.Get_Me().GetComponent<PlayerMovement>().isLocked = true;
        spawner.Get_Other().GetComponent<PlayerMovement>().isLocked = true;
        StartCoroutine(Open_ResultScreenCo());
    }

    private void Set_ResultScreen()
    {
        int timeElapsed = Mathf.FloorToInt(Time.time - startTime);
        int seconds = timeElapsed % 60;
        int minutes = timeElapsed / 60;
        string secs = (seconds < 10) ? "0" + seconds.ToString() : seconds.ToString();

        resultMembers[0].GetComponent<Text>().text = minutes.ToString() + ":" + secs;

        GameObject fireboy = (spawner.Get_Me().GetComponent<CollisionController>().element == Element.Fire) ? spawner.Get_Me() : spawner.Get_Other();
        int fireGemCount = fireboy.GetComponent<PlayerStats>().collectedGems;
        GameObject watergirl = (spawner.Get_Me().GetComponent<CollisionController>().element == Element.Water) ? spawner.Get_Me() : spawner.Get_Other();
        int waterGemCount = watergirl.GetComponent<PlayerStats>().collectedGems;

        resultMembers[1].GetComponent<Text>().text = fireGemCount.ToString();
        Text fireCheck = resultMembers[1].transform.Find("Check").GetComponent<Text>();

        fireCheck.text = (fireGemCount < expectations.fireGem) ? "X" : "✔";
        fireCheck.color = (fireGemCount < expectations.fireGem) ? Color.red : Color.green;


        resultMembers[2].GetComponent<Text>().text = waterGemCount.ToString();
        Text waterCheck = resultMembers[2].transform.Find("Check").GetComponent<Text>();

        waterCheck.text = (waterGemCount < expectations.waterGem) ? "X" : "✔";
        waterCheck.color = (waterGemCount < expectations.waterGem) ? Color.red : Color.green;

        resultMembers[3].SetActive(expectations.needBigGem);
        if (expectations.needBigGem)
        {
            Text bigGemCheck = resultMembers[3].transform.Find("Check").GetComponent<Text>();

            bigGemCheck.text = (fireboy.GetComponent<PlayerStats>().isBigGemCollected || watergirl.GetComponent<PlayerStats>().isBigGemCollected ? "✔" : "X");
            bigGemCheck.color = (bigGemCheck.text == "✔") ? Color.green : Color.red;
        }
    }

    private IEnumerator Open_ResultScreenCo()
    {
        yield return new WaitForSeconds(menuWait);
        resultScreen.SetActive(true);
    }

    public void Open_GameoverScreen()
    {
        StartCoroutine(Open_GameoverScreenCo());
    }

    private IEnumerator Open_GameoverScreenCo()
    {
        yield return new WaitForSeconds(menuWait);
        gameoverScreen.SetActive(true);
    }

    public void Update_Timer()
    {
        if (timer == null) return;
        
        int timeElapsed = Mathf.FloorToInt(Time.time - startTime);
        int seconds = timeElapsed % 60;
        int minutes = timeElapsed / 60;
        string secs = (seconds < 10) ? "0" + seconds.ToString() : seconds.ToString();

        timer.text = minutes.ToString() + ":" + secs;
    }
}


[System.Serializable]
public struct Expectation
{
    public float time;
    public int fireGem;
    public int waterGem;
    public bool needBigGem;
}
