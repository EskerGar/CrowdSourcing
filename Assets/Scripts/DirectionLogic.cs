using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionLogic : MonoBehaviour, IPunObservable
{
    public static bool direction;

    private void Start()
    {
        direction = false;
    }
    public void Direction()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Vector3 direction;
        Vector3 mousePos;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePos - transform.parent.position).normalized;
        var go = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.transform.position = go.position + direction;
        float dx = transform.position.x - mousePos.x;
        float dy = transform.position.y - mousePos.y;
        float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        transform.rotation = rot;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
            if (stream.IsWriting)
            {
                stream.SendNext(gameObject.GetComponent<SpriteRenderer>().enabled);
            }
            else
            {
                if(direction)
                    gameObject.GetComponent<SpriteRenderer>().enabled = (bool)stream.ReceiveNext();
            }
    }
}
