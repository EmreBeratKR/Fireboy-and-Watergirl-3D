using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    public GameObject me;
    public GameObject other;


    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (RoleSwitcher.MasterRole == Role.Fireboy)
            {
                PhotonNetwork.Instantiate("Fireboy", spawnPoints[0].position, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate("Watergirl", spawnPoints[1].position, Quaternion.identity);
            }
        }
        else
        {
            if (RoleSwitcher.MasterRole == Role.Fireboy)
            {
                PhotonNetwork.Instantiate("Watergirl", spawnPoints[1].position, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate("Fireboy", spawnPoints[0].position, Quaternion.identity);
            }
        }
    }

    public GameObject Get_Me()
    {
        if (me == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players[0].GetComponent<PhotonView>().IsMine)
            {
                me = players[0];
                other = players[1];
            }
            else
            {
                me = players[1];
                other = players[0];
            }
        }
        return me;
    }

    public GameObject Get_Other()
    {
        if (other == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (!players[0].GetComponent<PhotonView>().IsMine)
            {
                other = players[0];
                me = players[1];
            }
            else
            {
                other = players[1];
                me = players[0];
            }
        }
        return other;
    }
}
