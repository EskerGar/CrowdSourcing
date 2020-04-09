using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChooseServer : MonoBehaviourPunCallbacks
{
    public void ServerName()
    {
        var text = GameObject.FindGameObjectWithTag("username").GetComponent<Text>();
        SetNickName(text.text);
        var roomsName = gameObject.transform.parent.transform.parent.GetComponent<Text>().text.Substring(3);
        StartCoroutine(OnJoin(roomsName));
    }

    IEnumerator OnJoin(string roomsName)
    {
        yield return StartCoroutine(LobbyManager.client.GetID("Rooms/", roomsName));
        StartCoroutine(LobbyManager.client.PostPlayer(PhotonNetwork.NickName, LobbyManager.client.ID));
        PhotonNetwork.JoinRoom(roomsName);
    }
    private void SetNickName(string name)
    {
        if (name.Length != 0)
            PhotonNetwork.NickName = name;
    }
}
