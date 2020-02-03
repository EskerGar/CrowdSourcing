using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviourPunCallbacks
{
    private Rigidbody2D rb;
    private GameObject arrow;
    [SerializeField]
    private float force;
    private bool downed = false;
    private float gameTime;
    private int gameTimeID;
    private int dirVectID;
    private int playerID;
    private int RoomID;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        arrow = PhotonNetwork.Instantiate("arrow", new Vector3(0, -2f, 0), Quaternion.identity);
        arrow.name = PhotonNetwork.NickName;
        arrow.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform, false);
        arrow.transform.position = arrow.transform.parent.transform.position;
        RoomID = LobbyManager.client.GetID("Rooms/", PhotonNetwork.CurrentRoom.Name, "IDRoom");
        playerID = LobbyManager.client.GetID("Players/", PhotonNetwork.NickName, "IDPlayer");
        StartCoroutine(LogsPost());
    }

    [PunRPC]
    void Move(Vector2 direction) => rb.AddForce(direction * force, ForceMode2D.Impulse);

    private void OnMouseUp()
    {
        Vector3 mousePos;
        Vector2 direction;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePos - transform.position).normalized;
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("Move", RpcTarget.All, direction);
        arrow.GetComponent<SpriteRenderer>().enabled = false;
        downed = false;
        gameTimeID = GetGameTimeID();
        LobbyManager.client.PostDirectVector(direction.x, direction.y);
        dirVectID = LobbyManager.client.GetID("DirectionVectors/", direction.x.ToString() + "/" + direction.y.ToString() + "/", "IDDirectionVector");
        LobbyManager.client.PostImpact(gameTimeID, dirVectID, (int)force, playerID, RoomID);
    }

    private void OnMouseDown()
    {
        arrow.GetComponent<SpriteRenderer>().enabled = true;
        downed = true;
    }

    private void Update()
    {
        if (downed)
            arrow.GetComponent<DirectionLogic>().Direction();
    }

    IEnumerator LogsPost()
    {
        for (; ; )
        {
            Posts();
            yield return new WaitForSeconds(.2f);
        }
    }

    private void Posts()
    {
        gameTime = PhotonNetwork.ServerTimestamp / 1000;
        if (PhotonNetwork.IsMasterClient)
        { 
            gameTimeID = GetGameTimeID();
            LobbyManager.client.PostBall(gameObject.transform.position.x, gameObject.transform.position.y, rb.velocity.magnitude, gameTimeID);
        }
    }
    private int GetGameTimeID()
    {
        LobbyManager.client.PostGameTime(gameTime, RoomID);
        return  LobbyManager.client.GetID("GameTimes/", gameTime.ToString() + "/", "GameTimeID");
    }
}
