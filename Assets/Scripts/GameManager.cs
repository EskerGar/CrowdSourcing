﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{

    private void CreatePlayer()
    {
        PhotonNetwork.InstantiateSceneObject("player", new Vector3(0, -2f, 0), Quaternion.identity);
    }

    private void Start() => CreatePlayer();

    public void Leave() => PhotonNetwork.LeaveRoom();

    public override void OnLeftRoom() => SceneManager.LoadScene("StartScene");

    public override void OnPlayerEnteredRoom(Player newPlayer){}

    public override void OnPlayerLeftRoom(Player otherPlayer) => Debug.LogFormat("Player {0} left room", otherPlayer.NickName);
}
