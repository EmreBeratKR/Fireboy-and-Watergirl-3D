using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Transitor : MonoBehaviour
{
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(RestartLevel());
        }
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(0.1f);
        PhotonNetwork.LoadLevel(PlayerPrefs.GetInt("Restarted Level", LevelManager.levelStart));
    }
}
