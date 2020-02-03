using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using LogsClientAPI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Text userName;
    public Text currentServer;
    public Text ping;
    public Text roomName;
    private bool newJoin;
    private  Regions Regions;
    private Notices notice;
    public static LogsClient client;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        client = new LogsClient("http://localhost:60047/api/");
    }
    void Start()
    {
        currentServer.text = "Connecting...";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = "Player";
        newJoin = false;
        Regions = GetComponent<Regions>();
        notice = GetComponent<Notices>();
    }

    private void Update()
    {
        if(PhotonNetwork.IsConnected)
            ping.text = PhotonNetwork.GetPing().ToString();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if (!newJoin)
            PhotonNetwork.ConnectUsingSettings();
        else
            OnNewJoin();
    }

    public void OnNewJoin()
    {
        newJoin = true;
        var region = Regions.GetRegionShortName(currentServer.text);
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        currentServer.text = "Connecting...";
        PhotonNetwork.ConnectToRegion(region);
        notice.WaitToConnect();
    }

    public override void OnConnectedToMaster()
    {
        currentServer.text = Regions.GetRegionName(PhotonNetwork.CloudRegion);
        notice.Connected();
    }

    public void CreateRoom()
    {
        TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);
        string sqlLobbyFilter = "C0";
        SetNickName(userName.text);
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 5,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { sqlLobbyFilter, "0" } },
            CustomRoomPropertiesForLobby = new string[] { sqlLobbyFilter }
        };
        if (roomName.text.Length != 0)
        {
            client.PostRoom(roomName.text);
            int roomID = client.GetID("Rooms/", roomName.text, "IDRoom");
            client.PostPlayer(PhotonNetwork.NickName, roomID);
            PhotonNetwork.CreateRoom(roomName.text, roomOptions, sqlLobby);
        }
        else notice.EmptyRoomName();
    }
    public override void OnJoinedRoom()
    {
       PhotonNetwork.LoadLevel("Game");
    }

    private void SetNickName(string name)
    {
        if (name.Length != 0)
            PhotonNetwork.NickName = name;
    }
}
