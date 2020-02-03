using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisionUI : MonoBehaviour
{
    public void OnDirection()
    {
        var direct = DirectionLogic.direction;
        if(direct)
        { 
            GetComponentInChildren<Text>().text = "Vision Off";
            DirectionLogic.direction = false;
        }
        else 
        {
            GetComponentInChildren<Text>().text = "Vision On";
            DirectionLogic.direction = true;
        }
    }

}
