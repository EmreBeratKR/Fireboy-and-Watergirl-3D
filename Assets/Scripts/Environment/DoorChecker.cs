using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DoorChecker : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private Door[] doors;
    private bool isWon = false;


    private void Update()
    {
        CheckWin();
    }

    private void CheckWin()
    {
        if (!isWon && doors[0].isOpen && doors[1].isOpen)
        {
            isWon = true;
            sceneController.Open_ResultScreen();
        }
        else if (!isWon)
        {
            sceneController.Update_Timer();
        }
    }
}
