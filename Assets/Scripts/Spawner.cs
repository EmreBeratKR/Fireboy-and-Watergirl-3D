using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;


    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Fireboy", spawnPoints[0].position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Watergirl", spawnPoints[1].position, Quaternion.identity);
        }
    }
}
