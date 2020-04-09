using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notices : MonoBehaviour
{
    public Text notice;

    public void EmptyRoomName() => notice.text = "Room name is empty";
    public void Connected() => notice.text = "Connected";
    public void WaitToConnect() => notice.text = "Waiting to connect";

    public void RoomExist() => notice.text = "Room is already exist";
}
