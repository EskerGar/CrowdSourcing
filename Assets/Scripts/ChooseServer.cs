using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ChooseServer : MonoBehaviourPunCallbacks
{
    public void ServerName()
    {
        var text = GameObject.FindGameObjectWithTag("username").GetComponent<Text>();
        SetNickName(text.text);
        var roomsName = gameObject.transform.parent.transform.parent.GetComponent<Text>().text.Substring(3);
        int roomID = LobbyManager.client.GetID("Rooms/", roomsName, "IDRoom");
        LobbyManager.client.PostPlayer(PhotonNetwork.NickName, roomID);
        PhotonNetwork.JoinRoom(roomsName);
    }

    private void SetNickName(string name)
    {
        if (name.Length != 0)
            PhotonNetwork.NickName = name;
    }
}
