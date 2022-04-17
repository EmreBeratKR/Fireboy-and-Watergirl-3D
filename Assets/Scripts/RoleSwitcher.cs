using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RoleSwitcher : Singleton<RoleSwitcher>, IOnEventCallback
{
    [SerializeField] private Color fireboyColor;
    [SerializeField] private Color watergirlColor;
    private RoomController roomController;
    private Role masterRole;


    public static Role MasterRole { get => Instance.masterRole; set => Instance.masterRole = value; }
    

    public static Role OppositeRole(Role role)
    {
        return (role == Role.Fireboy) ? Role.Watergirl : Role.Fireboy;
    }

    public static Color RoleColor(Role role)
    {
        if (role == Role.Fireboy) return Instance.fireboyColor;

        return Instance.watergirlColor;
    }

    private void Start()
    {
        masterRole = Role.Fireboy;
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public static void OnRoleSwitched(Role role)
    {
        Instance.masterRole = role;

        if (Instance.roomController == null)
        {
            Instance.roomController = FindObjectOfType<RoomController>();
        }

        if (Instance.roomController == null) return;

        Instance.roomController.OnRoleSwitched();
    }

    public static void RaiseRoleSwitchedEvent()
    {
        object[] datas = new object[] {MasterRole};
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(EventCode._ROLESWITCHED_EVENTCODE, datas, options, SendOptions.SendUnreliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != EventCode._ROLESWITCHED_EVENTCODE) return;

        object[] datas = (object[]) photonEvent.CustomData;
        Role role = (Role) datas[0];

        OnRoleSwitched(role);
    }
}

public enum Role { Fireboy, Watergirl }