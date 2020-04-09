using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class PlayersImpact : MonoBehaviourPunCallbacks
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
    private LogsClientAPI client;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        client = GetComponent<LogsClientAPI>();
        client.Host = "https://newlogsserverapi20200402121056.azurewebsites.net/api/";
        arrow = PhotonNetwork.Instantiate("arrow", new Vector3(0, -2f, 100), Quaternion.identity);
        arrow.name = PhotonNetwork.NickName;
        arrow.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform, false);
        arrow.transform.position = arrow.transform.parent.transform.position;
        StartCoroutine(GetsID());
    }

    public override void OnLeftRoom()
    {
        if(!GameManager.Instanse.FinishMatch)
            StartCoroutine(client.PutRoom(PhotonNetwork.CurrentRoom.Name));
    }

    IEnumerator GetsID()
    {
        yield return StartCoroutine(client.GetID("Rooms/", PhotonNetwork.CurrentRoom.Name));
        RoomID = client.ID;
        yield return StartCoroutine(client.GetID("Players/", PhotonNetwork.NickName));
        playerID = client.ID;
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
        StartCoroutine(Impact(direction));
    }

    IEnumerator Impact(Vector2 direction)
    {
        yield return StartCoroutine(client.PostDirectVector(direction.x, direction.y));
        yield return StartCoroutine(GetGameTimeID());
        yield return StartCoroutine(client.GetID("DirectionVectors/", direction.x.ToString() + "/" + direction.y.ToString() + "/"));
        dirVectID = client.ID;
        StartCoroutine(client.PostImpact(gameTimeID, dirVectID, (int)force, playerID, RoomID));
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
            StartCoroutine(Posts());
            yield return new WaitForSeconds(.2f);
        }
    }
    IEnumerator Posts()
    { 
        if (PhotonNetwork.IsMasterClient)
        {
            yield return StartCoroutine(GetGameTimeID());
            yield return StartCoroutine(client.PostBall(gameObject.transform.position.x, gameObject.transform.position.y, rb.velocity.magnitude, gameTimeID));
            Debug.Log("Post Ball" + gameObject.transform.position.ToString());
        }
    }
    IEnumerator GetGameTimeID()
    {
        gameTime = PhotonNetwork.ServerTimestamp / 100;
        yield return StartCoroutine(client.PostGameTime(gameTime, RoomID));
        yield return StartCoroutine(client.GetID("GameTimes/", gameTime.ToString() + "/"));
        gameTimeID = client.ID;
    }
}
