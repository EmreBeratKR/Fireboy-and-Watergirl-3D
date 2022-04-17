using UnityEngine;
using Photon.Pun;

public class MasterOnlyButton : MonoBehaviour
{
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient) return;

        Destroy(gameObject);
    }
}
