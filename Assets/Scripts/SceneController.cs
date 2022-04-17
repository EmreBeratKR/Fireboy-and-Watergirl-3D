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
    [SerializeField] private Icons resultIcons;
    [SerializeField] private GameObject gameoverScreen;
    [SerializeField] private Expectation expectations;
    private Spawner spawner;
    private float startTime;
    private const float menuWait = 1.5f;


    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex >= LevelManager.levelStart)
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

    public void LoadWaitingRoom()
    {
        PhotonNetwork.LoadLevel(2); // Waiting Room Scene
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

    public void Load_Level(LevelNode levelNode)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(LevelManager.ToSceneIndex(levelNode.levelNumber));
        }
    }

    public void Restart_Level()
    {
        PlayerPrefs.SetInt("Restarted Level", SceneManager.GetActiveScene().buildIndex);
        PhotonNetwork.LoadLevel(4);
    }

    public void Toggle_PauseMenu(bool isOpen)
    {
        spawner.Get_Me().GetComponent<PlayerMovement>().isLocked = isOpen;
        pauseMenu.SetActive(isOpen);
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

        Image timeCheck = resultMembers[0].transform.Find("Check").GetComponent<Image>();

        bool timeSucceed = timeElapsed <= expectations.time;

        timeCheck.sprite = timeSucceed ? resultIcons.tick : resultIcons.cross;
        timeCheck.color = timeSucceed ? resultIcons.tickColor : resultIcons.crossColor;

        GameObject fireboy = (spawner.Get_Me().GetComponent<CollisionController>().element == Element.Fire) ? spawner.Get_Me() : spawner.Get_Other();
        int fireGemCount = fireboy.GetComponent<PlayerStats>().collectedGems;
        GameObject watergirl = (spawner.Get_Me().GetComponent<CollisionController>().element == Element.Water) ? spawner.Get_Me() : spawner.Get_Other();
        int waterGemCount = watergirl.GetComponent<PlayerStats>().collectedGems;

        resultMembers[1].SetActive(!expectations.needPureGem);
        resultMembers[2].SetActive(!expectations.needPureGem);

        resultMembers[3].SetActive(expectations.needPureGem);

        if (!expectations.needPureGem)
        {
            resultMembers[1].GetComponent<Text>().text = fireGemCount.ToString();
            Image fireCheck = resultMembers[1].transform.Find("Check").GetComponent<Image>();

            bool allFireGemsCollected = (fireGemCount >= expectations.fireGem);
            fireCheck.sprite = allFireGemsCollected ? resultIcons.tick : resultIcons.cross;
            fireCheck.color = allFireGemsCollected ? resultIcons.tickColor : resultIcons.crossColor;


            resultMembers[2].GetComponent<Text>().text = waterGemCount.ToString();
            Image waterCheck = resultMembers[2].transform.Find("Check").GetComponent<Image>();

            bool allWaterGemsCollected = (waterGemCount >= expectations.waterGem);
            waterCheck.sprite = allWaterGemsCollected ? resultIcons.tick : resultIcons.cross;
            waterCheck.color = allWaterGemsCollected ? resultIcons.tickColor : resultIcons.crossColor;

            int levelNumber = LevelManager.ToLevelNumber(SceneManager.GetActiveScene().buildIndex);
            LevelStatus levelStatus = LevelStatus.FullFinished;

            if (!timeSucceed)
            {
                levelStatus = LevelStatus.SemiFinished;
            }
            else if (!allFireGemsCollected)
            {
                levelStatus = LevelStatus.SemiFinished;
            }
            else if (!allWaterGemsCollected)
            {
                levelStatus = LevelStatus.SemiFinished;
            }

            MyEventSystem.RaiseLevelCompleted(levelNumber, levelStatus);
        }
        else
        {
            Image bigGemCheck = resultMembers[3].transform.Find("Check").GetComponent<Image>();
            bool isPureGemCollected = (fireboy.GetComponent<PlayerStats>().isPureGemCollected || watergirl.GetComponent<PlayerStats>().isPureGemCollected);
            bigGemCheck.sprite = isPureGemCollected ? resultIcons.tick : resultIcons.cross;
            bigGemCheck.color = isPureGemCollected ? resultIcons.tickColor : resultIcons.crossColor;

            int levelNumber = LevelManager.ToLevelNumber(SceneManager.GetActiveScene().buildIndex);
            LevelStatus levelStatus = isPureGemCollected ? LevelStatus.FullFinished : LevelStatus.NotFinished;

            MyEventSystem.RaiseLevelCompleted(levelNumber, levelStatus);
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




    [System.Serializable]
    private struct Icons
    {
        public Sprite waterGem;
        public Sprite fireGem;
        public Sprite pureGem;
        public Sprite tick;
        public Sprite cross;
        public Color tickColor;
        public Color crossColor;

    }
}


[System.Serializable]
public struct Expectation
{
    public float time;
    public int fireGem;
    public int waterGem;
    public bool needPureGem;
}
