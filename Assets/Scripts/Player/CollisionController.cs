using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CollisionController : EventListener
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerStats stats;
    private SceneController sceneController;
    public Element element;


    private void Start()
    {
        targetEvent = OnGameoverEvent;
        sceneController = FindObjectOfType<SceneController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckGameover(other.gameObject);

        if (other.tag == "Gem")
        {
            if (photonView.IsMine)
            {
                Gem gem = other.GetComponent<Gem>();
                if (gem.element == element)
                {
                    stats.collectedGems++;
                    gem.RaiseGemEvent();
                }
            }
        }
        else if (other.tag == "Switch")
        {
            if (photonView.IsMine)
            {
                other.GetComponent<Switch>().inRange = true;
            }
        }
        else if (other.tag == "Door")
        {
            Door door = other.GetComponent<Door>();
            if (element == door.element)
            {
                door.Raise_DoorEvent(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Switch")
        {
            if (photonView.IsMine)
            {
                other.GetComponent<Switch>().inRange = false;
            }
        }
        else if (other.tag == "Door")
        {
            Door door = other.GetComponent<Door>();
            if (element == door.element)
            {
                door.Raise_DoorEvent(false);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Lift")
        {
            playerMovement.isLifted = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Lift")
        {
            playerMovement.isLifted = false;
        }
    }

    private void CheckGameover(GameObject other)
    {
        if (photonView.IsMine)
        {
            switch (other.tag)
            {
                case "Water":
                    if (element == Element.Fire) Raise_GameoverEvent();
                    break;
                case "Lava":
                    if (element == Element.Water) Raise_GameoverEvent();
                    break;
                case "Acid":
                    Raise_GameoverEvent();
                    break;
            }
        }
    }

    private void OnGameoverEvent(EventData obj)
    {
        if (obj.Code == EventCode._GAMEOVER_EVENTCODE)
        {
            object[] datas = (object[]) obj.CustomData;
            int viewID = (int) datas[0];

            Gameover(viewID);
        }
    }

    public void Raise_GameoverEvent()
    {
        object[] datas = new object[] {photonView.ViewID};
        PhotonNetwork.RaiseEvent(EventCode._GAMEOVER_EVENTCODE, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        Gameover(photonView.ViewID);
    }

    private void Gameover(int id)
    {
        if (id == photonView.ViewID)
        {
            sceneController.Open_GameoverScreen();
            Destroy(gameObject);
        }
        else
        {
            playerMovement.isLocked = true;
        }
    }
}
