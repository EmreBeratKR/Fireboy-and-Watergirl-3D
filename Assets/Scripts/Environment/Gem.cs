using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Gem : EventListener
{
    public Element element;
    [SerializeField, Min(0f)] private float duration;
    [SerializeField, Min(0f)] private float deltaY;
    private Vector3 startPos;


    private void Start()
    {
        targetEvent = OnCollected;
        startPos = transform.localPosition;
        StartCoroutine(AnimationCo());
    }

    private void Collect()
    {
        Destroy(this.gameObject);
        AudioManager.PlayGemCollect();
    }

    private void OnCollected(EventData obj)
    {
        if (obj.Code == EventCode._GEMCOLLECT_EVENTCODE)
        {
            object[] datas = (object[]) obj.CustomData;
            int viewID = (int) datas[0];

            if (viewID == photonView.ViewID)
            {
                Collect();
            }
        }
    }

    public void RaiseGemEvent(bool isPureGem)
    {
        object[] datas = new object[] {photonView.ViewID, isPureGem};
        PhotonNetwork.RaiseEvent(EventCode._GEMCOLLECT_EVENTCODE, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        Collect();
    }

    private IEnumerator AnimationCo()
    {
        WaitForSeconds wait = new WaitForSeconds(duration);

        while (true)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            
            transform.LeanRotateY(-90f, duration);
            transform.LeanMoveLocalY(startPos.y + deltaY, duration).setEaseOutSine();
            yield return wait;

            transform.LeanRotateY(-180f, duration);
            transform.LeanMoveLocalY(startPos.y, duration).setEaseInSine();
            yield return wait;

            transform.LeanRotateY(-270f, duration);
            transform.LeanMoveLocalY(startPos.y - deltaY, duration).setEaseOutSine();
            yield return wait;

            transform.LeanRotateY(0f, duration);
            transform.LeanMoveLocalY(startPos.y, duration).setEaseInSine();
            yield return wait;
        }
    }
}

public enum Element {Fire, Water, Pure}
