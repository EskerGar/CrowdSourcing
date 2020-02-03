using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerList : MonoBehaviourPunCallbacks
{
    public RectTransform prefab;
    public RectTransform content;
    private TypedLobby sqlLobby;
    private List<GameObject> pullServers;
    void Start()
    {
        sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);
        pullServers = new List<GameObject>();
    }
    public void OnRefreshList()
    {
        PhotonNetwork.GetCustomRoomList(sqlLobby, "C0" + " = '0'");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (pullServers.Count != 0)
            foreach (var server in pullServers)
                Destroy(server);
        foreach (var list in roomList)
        {
            var instance = Instantiate(prefab.gameObject) as GameObject;
            instance.GetComponent<Text>().text = "   " + list.Name;
            instance.transform.GetChild(0).GetComponent<Text>().text = list.PlayerCount.ToString() + "/5";
            instance.transform.SetParent(content, false);
            pullServers.Add(instance);
        }
    }
}
