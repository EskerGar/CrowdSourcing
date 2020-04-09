using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Text userName;
    public Text currentServer;
    public Text ping;
    public Text roomName;
    private bool newJoin;
    private  Regions Regions;
    private Notices notice;
    public static LogsClientAPI client;
    private RoomOptions roomOptions;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        client = GetComponent<LogsClientAPI>();
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
    private IEnumerator ConfigureRoom(string roomNameText, TypedLobby sqlLobby)
    {
        yield return StartCoroutine(client.OnCheck(roomNameText));
        if (!client.Check)
        {
            yield return StartCoroutine(client.PostRoom(roomNameText, 1));
            yield return StartCoroutine(client.GetID("Rooms/", roomName.text));
            yield return StartCoroutine(client.PostPlayer(PhotonNetwork.NickName, client.ID));
            PhotonNetwork.CreateRoom(roomName.text, roomOptions, sqlLobby);
        }
        else notice.RoomExist();
    }
    public void CreateRoom()
    {
        TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);
        string sqlLobbyFilter = "C0";
        SetNickName(userName.text);
        roomOptions = new RoomOptions
        {
            MaxPlayers = 5,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { sqlLobbyFilter, "0" } },
            CustomRoomPropertiesForLobby = new string[] { sqlLobbyFilter }
        };
        if (roomName.text.Length != 0)
            StartCoroutine(ConfigureRoom(roomName.text, sqlLobby));
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
