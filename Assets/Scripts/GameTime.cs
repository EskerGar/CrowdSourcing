using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviourPunCallbacks
{

    void Update()
    {
        gameObject.GetComponent<Text>().text = (PhotonNetwork.Time).ToString();
    }
}
