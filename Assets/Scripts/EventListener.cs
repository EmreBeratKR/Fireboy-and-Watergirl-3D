using Photon.Pun;
using ExitGames.Client.Photon;

public abstract class EventListener : MonoBehaviourPun
{
    public delegate void EventDel(EventData obj);
    public EventDel targetEvent;
    
    protected virtual void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    protected virtual void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }

    private void OnEventReceived(EventData obj)
    {
        if (targetEvent == null) return;
        targetEvent(obj);
    }
}
