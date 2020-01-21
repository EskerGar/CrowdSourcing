using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject arrow;
    [SerializeField]
    private float force;
    private bool downed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        arrow = PhotonNetwork.Instantiate("arrow", new Vector3(0, -2f, 0), Quaternion.identity);
        arrow.name = PhotonNetwork.NickName;
        arrow.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
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
}
